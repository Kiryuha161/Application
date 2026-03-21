import axios from 'axios';

const API_URL = 'https://localhost:7171/api';

const api = axios.create({
    baseURL: API_URL,
    headers: {
        'Content-Type': 'application/json',
    },
});

api.interceptors.request.use((config) => {
    const token = localStorage.getItem('token');
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

export const userApi = {
    getAll: () => api.get('/identity/users'),
    getById: (id: string) => api.get(`/identity/users/${id}`),
    getCurrent: () => api.get('/identity/users/me'),
};

export default api;