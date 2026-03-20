import React, { useState } from "react";
import { auth } from "../api/auth";

interface RegisterProps {
    onSuccess: () => void;
    onSwitchToLogin: () => void;
}

const Register: React.FC<RegisterProps> = ({ onSuccess, onSwitchToLogin }) => {
    const [form, setForm] = useState();
    const [error, setError] = useState<string>();
    const [loading, setLoading] = useState<boolean>();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        setError("");

        try {
            const response = await auth.register(form);
            console.log("response", response);
            localStorage.setItem("token", response.data?.token);
            onSuccess();
        } catch (error: any) {
            console.log("error", error);
            setError(error.response?.data?.error || "Ошибка регистрации");
        } finally {
            setLoading(false);
        }
    }

    return (
        <div style={{ maxWidth: 400, margin: "0 auto", padding: 20 }}>
            <h2>Регистрация</h2>
            {error && <div style={{ color: "red", marginBottom: 10 }}>{error}</div>}
            <form onSubmit={handleSubmit}>
                <div style={{ marginBottom: 10 }}>
                    <input type="email"
                        placeholder="Email"
                        value={form?.email || ""}
                        onChange={e => setForm({ ...form, email: e.target.value })}
                        required
                        style={{ width: "100%", padding: 8, boxSizing: "border-box" }}
                    >
                    </input>
                </div>
                <div style={{ marginBottom: 10 }}>
                    <input type="password"
                        placeholder="Пароль"
                        value={form?.password || ""}
                        onChange={e => setForm({ ...form, password: e.target.value })}
                        required
                        style={{ width: "100%", padding: 8, boxSizing: "border-box" }}
                    >
                    </input>
                </div>
                <button type="submit"
                    disabled={loading}
                    style={{ width: '100%', padding: 10, background: '#007bff', color: 'white', border: 'none' }}
                >
                    { loading ? "Загрузка..." : "Зарегистрироваться" }
                </button>
            </form>

            <p style={{marginTop: 10, textAlign: "center"}}>
                Уже есть аккаунт? <span onClick={onSwitchToLogin} style={{ color: "#007bff", cursor: "pointer" }}>Войти</span>
            </p>
        </div>
    )
}

export default Register;