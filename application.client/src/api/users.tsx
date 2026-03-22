import { userApi } from "../api/client";

export const usersApi = {
    getAll: () => userApi.get('/'),
    getById: (id: string) => userApi.get(`/${id}`),
    getCurrent: () => userApi.get('/me'),
};

export default usersApi;