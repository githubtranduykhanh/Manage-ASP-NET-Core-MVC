import axios from 'https://cdn.skypack.dev/axios';
// Tạo một instance Axios
const axiosInstance = axios.create({ baseURL:"https://localhost:7069/"});


// Tạo một interceptor cho Axios
axiosInstance.interceptors.request.use(
    (config) => {
        // Kiểm tra xem token có tồn tại trong cookie không
        const token = getItem('accessToken');
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
        // Xử lý các lỗi response ở đây
        // Ví dụ: Hiển thị một thông báo lỗi
        console.error('Error:', error);

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
const getItem = (name) => {
    return localStorage.getItem(name);
}


export default axiosInstance;
