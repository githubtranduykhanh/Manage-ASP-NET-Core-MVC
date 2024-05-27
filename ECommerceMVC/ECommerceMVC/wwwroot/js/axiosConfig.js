import axios from 'https://cdn.skypack.dev/axios';
// Tạo một instance Axios
const axiosInstance = axios.create({ baseURL:"https://localhost:7069"});
let refreshTokenPromise = null;

// Tạo một interceptor cho Axios
axiosInstance.interceptors.request.use(
    (config) => {
        // Kiểm tra xem token có tồn tại trong cookie không
        const token = getItemLocalStorage('accessToken');
        // Nếu có, gắn token vào header Authorization
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);


// Thêm interceptor cho response
axiosInstance.interceptors.response.use(
    (response) => {
        // Xử lý các response thành công ở đây, nếu cần
        return response?.data;
    },
    (error) => {       
        console.error('Error:', error);       
        if (error.response.status === 401) {
            const wwwAuthenticateHeader = error.response.headers['www-authenticate'];
            // Kiểm tra nếu chưa có promise làm mới token hoặc promise trước đó đã thực hiện xong
            if (wwwAuthenticateHeader && wwwAuthenticateHeader.includes('The token expired')) {
                if (!refreshTokenPromise) {
                    // Tạo một promise để làm mới token
                    refreshTokenPromise = new Promise((resolve, reject) => {
                        axiosInstance({
                            url: '/Account/RefreshToken',
                            method: 'get',
                            withCredentials: true, //Lưu cookie trên trình duyệt cùng với cấu hình credentials:true server
                        }).then((response) => {
                            saveTokenLocalStorage(response?.newAccessToken)
                            resolve(); // Hoàn thành promise khi token được cập nhật
                        }).catch((errorRefreshTokenPromise) => {
                            // Làm mới token không thành công, xử lý lỗi và từ chối promise
                            destroyTokenLocalStorage();
                            window.location.href = "/Account/Login"
                            reject(errorRefreshTokenPromise);
                        }).finally(() => {
                            refreshTokenPromise = null // Đặt lại promise sau khi hoàn thành
                        })
                    })
                }
               
                return refreshTokenPromise.then(() => {
                    // Sau khi token đã được cập nhật, gọi lại yêu cầu ban đầu, nhưng với token đã được cập nhật trong headers
                    error.response.config.headers["Authorization"] = `Bearer ${getItemLocalStorage('accessToken')}`
                    return axiosInstance(error.response.config)
                }).catch((ex) => {
                    // Nếu có lỗi trong quá trình làm mới token, từ chối promise và đưa ra lỗi
                    return Promise.reject(ex)
                });
            }
            if (wwwAuthenticateHeader && wwwAuthenticateHeader.includes('invalid_token')) {
                destroyTokenLocalStorage();
                window.location.href = "/Account/Login"
                return Promise.reject(error)
            }
            if (wwwAuthenticateHeader && wwwAuthenticateHeader.includes('Bearer')) {
                destroyTokenLocalStorage();
                window.location.href = "/Account/Login"
                return Promise.reject(error)
            }
            destroyTokenLocalStorage();
            window.location.href = "/Account/Login"
            return Promise.reject(error);
        }

        // Để promise tiếp tục bị reject và đưa ra cho phần gọi xử lý
        return Promise.reject(error);
    }
);

// Hàm để lấy giá trị của cookie
const getCookie = (name) => {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
}

// Hàm để lấy giá trị từ localStorage
const getItemLocalStorage = (name) => {
    return localStorage.getItem(name);
}


// Hàm để xóa token
const destroyTokenLocalStorage = () => {
    localStorage.removeItem('accessToken');
};


// Hàm để lưu token vào local storage
const saveTokenLocalStorage = (token) => {
    localStorage.setItem('accessToken', token);
};

export default axiosInstance;
