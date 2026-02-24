import React, { useState } from 'react';
import api from '../services/api';
import { useNavigate } from 'react-router-dom';

const RegisterPage: React.FC = () => {
    const [username, setUsername] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');
    const navigate = useNavigate();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError('');
        setSuccess('');
        try {
            await api.post('/auth/register', { username, email, password });
            setSuccess('Registration successful! Redirecting to login...');
            setTimeout(() => navigate('/login'), 2000);
        } catch (err) {
            setError('Registration failed. Please try again.');
        }
    };

    return (
        <div className="login-page-bg">
            <div className="login-card">
                <h2 className="login-title">Register</h2>
                <form onSubmit={handleSubmit} className="login-form">
                    <div className="login-field">
                        <label htmlFor="username">Username</label>
                        <input
                            id="username"
                            type="text"
                            value={username}
                            onChange={e => setUsername(e.target.value)}
                            required
                            className="login-input"
                            autoComplete="username"
                        />
                    </div>
                    <div className="login-field">
                        <label htmlFor="email">Email</label>
                        <input
                            id="email"
                            type="email"
                            value={email}
                            onChange={e => setEmail(e.target.value)}
                            required
                            className="login-input"
                            autoComplete="email"
                        />
                    </div>
                    <div className="login-field">
                        <label htmlFor="password">Password</label>
                        <input
                            id="password"
                            type="password"
                            value={password}
                            onChange={e => setPassword(e.target.value)}
                            required
                            className="login-input"
                            autoComplete="new-password"
                        />
                    </div>
                    <button type="submit" className="login-btn">Register</button>
                    {error && <p className="login-error">{error}</p>}
                    {success && <p className="login-success">{success}</p>}
                </form>
            </div>
        </div>
    );
};

export default RegisterPage;
