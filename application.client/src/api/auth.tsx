import axios from "axios";
const API_URL = "https://localhost:7171/api/identity/auth";

const api = axios.create({
    baseURL: API_URL,
    headers: {
        "Content-Type": "application/json"
    }
});

api.interceptors.request.use((config) => {
    const token = localStorage.getItem("token");

    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
})

export const auth = {
    register: (data: { email: string, password: string }) =>
        api.post("/register", data),
    login: (data: { email: string, password: string }) =>
        api.post("/login", data)
}