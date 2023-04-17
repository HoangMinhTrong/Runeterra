import axios from 'axios';

// Tạo instance Axios
const instance = axios.create();

// Thêm interceptor để đưa token vào header Authorization
instance.interceptors.request.use(
    config => {
        const token = localStorage.getItem('token');
        if (token) {
            config.headers['Authorization'] = `Bearer ${token}`;
        }
        return config;
    },
    error => {
        Promise.reject(error);
    }
);

export default instance;