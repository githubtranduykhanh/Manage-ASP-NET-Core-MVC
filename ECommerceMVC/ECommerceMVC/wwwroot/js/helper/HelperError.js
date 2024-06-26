export const ConsoleErrorCatch = (error) => { 
    if (error.response) {
        // Request đã được gửi và server đã trả về response với status code không thành công
        console.log('Server responded with non-success status error:', error.response.status);
        console.log('Response data error:', error.response.data);
        error?.response?.data?.errors?.forEach(error => {
            console.log(`Validation error: ${error}`);
        });
    } else if (error.request) {
        // Request đã được gửi nhưng không nhận được response (có thể do mạng hoặc server không phản hồi)
        console.error('Request sent but no response received:', error.request);
    } else {
        // Lỗi xảy ra khi thiết lập request
        console.error('Error setting up the request:', error.message);
    }
}


export const ConsoleErrorStatus = (errors) => {
    errors?.forEach(error => {
        console.log(`Validation error: ${error}`);
    });    
}