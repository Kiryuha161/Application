import React, { useState } from 'react';
import Login from '../components/Login';
import Register from '../components/Register';

interface AuthPageProps {
    onAuthSuccess: () => void;
}

const AuthPage: React.FC<AuthPageProps> = ({ onAuthSuccess }) => {
    const [isLogin, setIsLogin] = useState(true);

    return (
        <div style={{
            minHeight: 'calc(100vh - 64px)',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            backgroundColor: '#f5f5f5'
        }}>
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
    );
};

export default AuthPage;