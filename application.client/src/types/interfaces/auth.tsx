import type { IdentityUser } from "../interfaces/user";

// Типы для авторизации
export type AuthMode = 'login' | 'register';

// Тип для состояния авторизации в App
export interface AuthState {
    isAuthenticated: boolean;
    user: IdentityUser | null;
    showAuth: boolean;
    authMode: AuthMode;
}