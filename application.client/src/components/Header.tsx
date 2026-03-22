import React from "react";
import styles from './Header.module.css';

interface HeaderProps {
    isAuthenticated: boolean;
    userEmail: string | null;
    onLogout: () => void;
    onLoginClick: () => void;
    onRegisterClick: () => void;
}

const Header: React.FC<HeaderProps> = ({
    isAuthenticated,
    userEmail,
    onLogout,
    onLoginClick,
    onRegisterClick
}) => {
    return (
        <header className={styles.header}>
            <div className={styles.logo}>
                Приложение
            </div>
            <nav>
                {isAuthenticated ? (
                    <div className={styles.userInfo}>
                        <span className={styles.welcome}>
                            Добро пожаловать, {userEmail}!
                        </span>
                        <button
                            onClick={onLogout}
                            className={styles.logoutButton}
                        >
                            Выход
                        </button>
                    </div>
                ) : (
                    <div className={styles.authButtons}>
                        <button
                            onClick={onLoginClick}
                            className={styles.loginButton}
                        >
                            Вход
                        </button>
                        <button
                            onClick={onRegisterClick}
                            className={styles.registerButton}
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