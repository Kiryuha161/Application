import React from 'react';
import UserList from '../components/UserList';

const HomePage: React.FC = () => {
    return (
        <div style={{
            padding: '2rem',
            maxWidth: '1200px',
            margin: '0 auto'
        }}>
            <UserList />
        </div>
    );
};

export default HomePage;