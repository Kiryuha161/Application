import axios from "axios";

const BASE_API_URL = "https://localhost:7171/api";

const createApiClient = (baseUrl: string) => {
    const client = axios.create({
        baseURL: `${BASE_API_URL}${baseUrl}`,
        headers: {
            "Content-Type": "application/json",
        },
    });

    client.interceptors.request.use((config) => {
        const token = localStorage.getItem("token");
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    });

    return client;
};

export const authApi = createApiClient("/identity/auth");
export const userApi = createApiClient("/identity/users");
export const profileApi = createApiClient("/users/profile");

export default createApiClient;