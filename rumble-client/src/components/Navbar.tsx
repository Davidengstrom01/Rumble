import React from 'react';
import { Link } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

const Navbar: React.FC = () => {
    const { token, logout } = useAuth();

    return (
        <nav>
            <Link to="/">Home</Link> | <Link to="/recipes">Recipes</Link> | <Link to="/friends">Friends</Link> | <Link to="/swipe">Swipe</Link>
            {token ? (
                <button onClick={logout}>Logout</button>
            ) : (
                <>
                    | <Link to="/login">Login</Link> | <Link to="/register">Register</Link>
                </>
            )}
        </nav>
    );
};

export default Navbar;
