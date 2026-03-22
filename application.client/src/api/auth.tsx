import { authApi } from "../api/client";

export const auth = {
    register: (data: { email: string, password: string }) =>
        authApi.post("/register", data),
    login: (data: { email: string, password: string }) =>
        authApi.post("/login", data)
}