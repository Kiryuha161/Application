import { useEffect, useState } from 'react';
import './App.css';
import Header from './components/Header';
import { usersApi } from './api/users';
import AuthPage from './components/AuthPage';
import HomePage from './components/HomePage';
import type { User, AuthMode } from './types';

function App() {
    const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
    const [user, setUser] = useState<User | null>(null);
    const [showAuth, setShowAuth] = useState<boolean>(false);
    const [authMode, setAuthMode] = useState<AuthMode>('login');

    useEffect(() => {
        checkAuth();
    }, []);

    const handleLogout = () => {
        localStorage.removeItem("token");
        setIsAuthenticated(false);
        setUser(null);
        setShowAuth(true);
        setAuthMode('login');
    }

    const handleLoginClick = () => {
        setAuthMode('login');
        setShowAuth(true);
    }

    const handleRegisterClick = () => {
        setAuthMode('register');
        setShowAuth(true);
    }

    const checkAuth = async () => {
        const token = localStorage.getItem('token');
        if (token) {
            try {
                const response = await usersApi.getCurrent();
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
        <div className="app">
            <Header
                isAuthenticated={isAuthenticated}
                userEmail={user?.email}
                onLogout={handleLogout}
                onLoginClick={handleLoginClick}
                onRegisterClick={handleRegisterClick}
            />
            <main className="main">
                {!isAuthenticated && showAuth ? (
                    <AuthPage
                        key={authMode}
                        onAuthSuccess={checkAuth}
                        initialMode={authMode}
                    />
                ) : isAuthenticated ? (
                    <HomePage />
                ) : null}
            </main>
        </div>
    )
}

export default App;