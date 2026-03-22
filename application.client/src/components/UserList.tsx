import React, { useState, useEffect } from 'react';
import { usersApi } from '../api/users';

interface UserDto {
    id: string;
    email: string;
    username: string;
}

const UsersList: React.FC = () => {
    const [users, setUsers] = useState<UserDto[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');

    useEffect(() => {
        loadUsers();
    }, []);

    const loadUsers = async () => {
        try {
            const response = await usersApi.getAll();
            setUsers(response.data);
        } catch (err: any) {
            setError(err.response?.data?.error || 'Ошибка загрузки пользователей');
        } finally {
            setLoading(false);
        }
    };

    if (loading) {
        return (
            <div style={{ textAlign: 'center', padding: '2rem' }}>
                Загрузка пользователей...
            </div>
        );
    }

    if (error) {
        return (
            <div style={{ textAlign: 'center', padding: '2rem', color: '#e74c3c' }}>
                ❌ {error}
            </div>
        );
    }

    if (users.length === 0) {
        return (
            <div style={{ textAlign: 'center', padding: '2rem', color: '#7f8c8d' }}>
                Пользователи не найдены
            </div>
        );
    }

    return (
        <div>
            <h2 style={{ marginBottom: '1rem', color: '#2c3e50' }}>
                Список пользователей ({users.length})
            </h2>
            <div style={{ overflowX: 'auto' }}>
                <table style={{ width: '100%', borderCollapse: 'collapse' }}>
                    <thead>
                        <tr style={{ background: '#34495e', color: 'white' }}>
                            <th style={{ padding: '12px', border: '1px solid #3b4f63', textAlign: 'left' }}>
                                Имя пользователя
                            </th>
                            <th style={{ padding: '12px', border: '1px solid #3b4f63', textAlign: 'left' }}>
                                Email
                            </th>
                            <th style={{ padding: '12px', border: '1px solid #3b4f63', textAlign: 'left' }}>
                                ID
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        {users.map((user, index) => (
                            <tr
                                key={user.id}
                                style={{
                                    background: index % 2 === 0 ? '#f9f9f9' : 'white',
                                    transition: 'background 0.2s',
                                }}
                                onMouseEnter={(e) => {
                                    e.currentTarget.style.background = '#ecf0f1';
                                }}
                                onMouseLeave={(e) => {
                                    e.currentTarget.style.background = index % 2 === 0 ? '#f9f9f9' : 'white';
                                }}
                            >
                                <td style={{ padding: '10px', border: '1px solid #ddd' }}>
                                    <strong>{user.username}</strong>
                                </td>
                                <td style={{ padding: '10px', border: '1px solid #ddd' }}>
                                    {user.email}
                                </td>
                                <td style={{ padding: '10px', border: '1px solid #ddd', fontFamily: 'monospace', fontSize: '12px' }}>
                                    {user.id.slice(0, 8)}...
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
};

export default UsersList;