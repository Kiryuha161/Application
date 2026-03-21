import { useEffect, useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'
import Register from './components/Register'
import Header from './components/Header'
import { auth } from './api/auth'
import { userApi } from './api/users'
import AuthPage from './components/AuthPage'
import HomePage from './components/HomePage'

function App() {
    const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
    const [user, setUser] = useState<{ email: string } | null>(null);
    const [showAuth, setShowAuth] = useState<boolean>(false);

    useEffect(() => {
        checkAuth();
    }, []);

    const onSuccess = () => {

    }

    const onSwitchToLogin = () => {

    }

    const handleLogout = () => {
        localStorage.removeItem("token");
        setIsAuthenticated(false);
        setUser(null);
        setShowAuth(true);
    }

    const handleLoginClick = () => {
        setShowAuth(true);
    }

    const handleRegisterClick = () => {
        setShowAuth(true);
    }

    const checkAuth = async () => {
        const token = localStorage.getItem('token');
        if (token) {
            try {
                const response = await userApi.getCurrent();
                setUser(response.data);
                setIsAuthenticated(true);
                setShowAuth(false);
            } catch {
                localStorage.removeItem('token');
                setIsAuthenticated(false);
                setShowAuth(true);
            }
        } else {
            setShowAuth(true);
        }
    };

    return (
        <>
            <Header
                isAuthenticated={isAuthenticated}
                userEmail={user?.email}
                onLogout={handleLogout}
                onLoginClick={handleLoginClick}
                onRegisterClick={handleRegisterClick}
            />

            {!isAuthenticated && showAuth ? (
                <AuthPage onAuthSuccess={checkAuth} />
            ) : isAuthenticated ? (
                <HomePage />
            ) : null}
        </>
    )
}

export default App
