// Типы для пользователя (Identity модуль)
export interface IdentityUser {
    id: string;
    email: string;
    username: string;
}

// Тип для ответа при регистрации/логине
export interface AuthResponse {
    id: string;
    token: string;
    user: IdentityUser;
}

// Тип для запроса регистрации
export interface RegisterRequest {
    username: string;
    email: string;
    password: string;
}

// Тип для запроса логина
export interface LoginRequest {
    email: string;
    password: string;
}