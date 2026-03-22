import React, { useState } from 'react';
import { auth } from '../api/auth';
import type { LoginRequest } from '../types/interfaces/user';
import styles from './Login.module.css';

interface LoginProps {
    onSuccess: () => void;
    onSwitchToRegister: () => void;
}

const Login: React.FC<LoginProps> = ({ onSuccess, onSwitchToRegister }) => {
    const [form, setForm] = useState<LoginRequest>({ email: '', password: '' });
    const [error, setError] = useState<string>('');
    const [loading, setLoading] = useState<boolean>(false);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        setError('');

        try {
            const response = await auth.login(form);
            localStorage.setItem('token', response.data.token);
            onSuccess();
        } catch (err: any) {
            setError(err.response?.data?.error || 'Ошибка входа');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className={styles.loginForm}>
            <h2 className={styles.title}>Вход</h2>
            {error && <div className={styles.error}>{error}</div>}
            <form onSubmit={handleSubmit}>
                <div className={styles.formGroup}>
                    <input
                        type="email"
                        placeholder="Email"
                        value={form.email}
                        onChange={(e) => setForm({ ...form, email: e.target.value })}
                        required
                        className={styles.input}
                    />
                </div>
                <div className={styles.formGroup}>
                    <input
                        type="password"
                        placeholder="Пароль"
                        value={form.password}
                        onChange={(e) => setForm({ ...form, password: e.target.value })}
                        required
                        className={styles.input}
                    />
                </div>
                <button
                    type="submit"
                    disabled={loading}
                    className={styles.button}
                >
                    {loading ? 'Загрузка...' : 'Войти'}
                </button>
            </form>
            <div className={styles.footer}>
                Нет аккаунта?
                <button onClick={onSwitchToRegister} className={styles.switchButton}>
                    Зарегистрироваться
                </button>
            </div>
        </div>
    );
};

export default Login;