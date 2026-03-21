import React, { useState } from 'react';
import { auth } from '../api/auth';

interface LoginProps {
    onSuccess: () => void;
    onSwitchToRegister: () => void;
}

const Login: React.FC<LoginProps> = ({ onSuccess, onSwitchToRegister }) => {
    const [form, setForm] = useState({ email: '', password: '' });
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);

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
        <div style={{ maxWidth: 400, width: '100%', padding: 20, background: 'white', borderRadius: 8, boxShadow: '0 2px 8px rgba(0,0,0,0.1)' }}>
            <h2 style={{ textAlign: 'center', marginBottom: 20 }}>Вход</h2>
            {error && <div style={{ color: 'red', marginBottom: 10, textAlign: 'center' }}>{error}</div>}
            <form onSubmit={handleSubmit}>
                <div style={{ marginBottom: 15 }}>
                    <input
                        type="email"
                        placeholder="Email"
                        value={form.email}
                        onChange={(e) => setForm({ ...form, email: e.target.value })}
                        required
                        style={{ width: '100%', padding: 10, border: '1px solid #ddd', borderRadius: 4 }}
                    />
                </div>
                <div style={{ marginBottom: 15 }}>
                    <input
                        type="password"
                        placeholder="Пароль"
                        value={form.password}
                        onChange={(e) => setForm({ ...form, password: e.target.value })}
                        required
                        style={{ width: '100%', padding: 10, border: '1px solid #ddd', borderRadius: 4 }}
                    />
                </div>
                <button
                    type="submit"
                    disabled={loading}
                    style={{ width: '100%', padding: 10, background: '#3498db', color: 'white', border: 'none', borderRadius: 4, cursor: 'pointer' }}
                >
                    {loading ? 'Загрузка...' : 'Войти'}
                </button>
            </form>
            <p style={{ marginTop: 15, textAlign: 'center' }}>
                Нет аккаунта?{' '}
                <button onClick={onSwitchToRegister} style={{ background: 'none', border: 'none', color: '#3498db', cursor: 'pointer' }}>
                    Зарегистрироваться
                </button>
            </p>
        </div>
    );
};

export default Login;