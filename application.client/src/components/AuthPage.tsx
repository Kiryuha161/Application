import React, { useEffect, useState } from 'react';
import Login from './Login';
import Register from './Register';
import type { AuthMode } from "../types/interfaces/auth";
import { AuthModes } from "../types/constant/auth";
import styles from './AuthPage.module.css';

interface AuthPageProps {
    onAuthSuccess: () => void;
    initialMode?: AuthMode; 
}

const AuthPage: React.FC<AuthPageProps> = ({ onAuthSuccess, initialMode = AuthModes.Login }) => {
    const [isLogin, setIsLogin] = useState(initialMode === AuthModes.Login);

    useEffect(() => {
        setIsLogin(initialMode === AuthModes.Login);
    }, [initialMode]);

    return (
        <div className={styles.authPage}>
            <div className={styles.container}>
                <div className={styles.formWrapper}>
                    {isLogin ? (
                        <Login
                            onSuccess={onAuthSuccess}
                            onSwitchToRegister={() => setIsLogin(false)}
                        />
                    ) : (
                        <Register
                            onSuccess={onAuthSuccess}
                            onSwitchToLogin={() => setIsLogin(true)}
                        />
                    )}
                </div>
            </div>
        </div>
    );
};

export default AuthPage;