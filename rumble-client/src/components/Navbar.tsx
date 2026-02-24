import React from 'react';
import { Link } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

const Navbar: React.FC = () => {
    const { token, logout } = useAuth();

    return (
        <nav className="navbar">
            <div className="navbar-content">
                <Link to="/">Home</Link>
                <span> | </span>
                <Link to="/recipes">Recipes</Link>
                <span> | </span>
                <Link to="/friends">Friends</Link>
                <span> | </span>
                <Link to="/swipe">Swipe</Link>
                {token ? (
                    <>
                        <span> | </span>
                        <button onClick={logout}>Logout</button>
                    </>
                ) : (
                    <>
                        <span> | </span>
                        <Link to="/login">Login</Link>
                        <span> | </span>
                        <Link to="/register">Register</Link>
                    </>
                )}
            </div>
        </nav>
    );
};

export default Navbar;
