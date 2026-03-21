import React from "react";

interface HeaderProps {
    isAuthenticated: boolean;
    userEmail: string | null;
    onLogout: () => void;
    onLoginClick: () => void;
    onRegisterClick: () => void;
}

const Header: React.FC<HeaderProps> = ({ isAuthenticated, userEmail, onLogout, onLoginClick, onRegisterClick }) => {
    return (
        <header style={{
            backgroundColor: '#2c3e50',
            color: 'white',
            padding: '1rem 2rem',
            display: 'flex',
            justifyContent: 'space-between',
            alignItems: 'center',
            boxShadow: '0 2px 4px rgba(0,0,0,0.1)'
        }}>

            <div style={{ fontSize: '1.5rem', fontWeight: 'bold' }}>
                Приложение
            </div>
            <nav>
                {isAuthenticated ? (
                    <div style={{ display: 'flex', alignItems: 'center', gap: '1rem' }}>
                        <span>Добро пожаловать, {userEmail}!</span>
                        <button
                            onClick={onLogout}
                            style={{
                                padding: '0.5rem 1rem',
                                backgroundColor: '#e74c3c',
                                color: 'white',
                                border: 'none',
                                borderRadius: '4px',
                                cursor: 'pointer',
                                fontSize: '14px'
                            }}
                        >
                            Выход
                        </button>
                    </div>
                ) : (
                    <div style={{ display: 'flex', gap: '1rem' }}>
                            <button
                                onClick={onLoginClick}
                                style={{
                                    padding: '0.5rem 1rem',
                                    backgroundColor: '#3498db',
                                    color: 'white',
                                    border: 'none',
                                    borderRadius: '4px',
                                    cursor: 'pointer',
                                    fontSize: '14px'
                                }}
                            >
                                Вход
                            </button>
                            <button
                                onClick={onRegisterClick}
                                style={{
                                    padding: '0.5rem 1rem',
                                    backgroundColor: '#2ecc71',
                                    color: 'white',
                                    border: 'none',
                                    borderRadius: '4px',
                                    cursor: 'pointer',
                                    fontSize: '14px'
                                }}
                            >
                                Регистрация
                            </button>
                    </div>
                )}
            </nav>
        </header>
    );
};

export default Header;