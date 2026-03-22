import React, { useState } from 'react';
import { auth } from '../api/auth';
import type { RegisterRequest } from '../types/interfaces/user';
import styles from './Register.module.css';

interface RegisterProps {
    onSuccess: () => void;
    onSwitchToLogin: () => void;
}

const Register: React.FC<RegisterProps> = ({ onSuccess, onSwitchToLogin }) => {
    const [form, setForm] = useState<RegisterRequest>({ username: '', email: '', password: '' });
    const [error, setError] = useState<string>('');
    const [loading, setLoading] = useState<boolean>(false);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        setError('');

        try {
            const response = await auth.register(form);
            localStorage.setItem('token', response.data.token);
            onSuccess();
        } catch (err: any) {
            setError(err.response?.data?.error || 'Ошибка регистрации');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className={styles.registerForm}>
            <h2 className={styles.title}>Регистрация</h2>
            {error && <div className={styles.error}>{error}</div>}
            <form onSubmit={handleSubmit}>
                <div className={styles.formGroup}>
                    <input
                        type="text"
                        placeholder="Имя пользователя"
                        value={form.username}
                        onChange={(e) => setForm({ ...form, username: e.target.value })}
                        required
                        className={styles.input}
                    />
                </div>
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
                    {loading ? 'Загрузка...' : 'Зарегистрироваться'}
                </button>
            </form>
            <div className={styles.footer}>
                Уже есть аккаунт?
                <button onClick={onSwitchToLogin} className={styles.switchButton}>
                    Войти
                </button>
            </div>
        </div>
    );
};

export default Register;