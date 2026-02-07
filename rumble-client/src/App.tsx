import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';
import HomePage from './pages/HomePage';
import Navbar from './components/Navbar';
import { useAuth } from './context/AuthContext';

// A simple component to protect routes
const PrivateRoute = ({ children }: { children: JSX.Element }) => {
    const { token } = useAuth();
    return token ? children : <Navigate to="/login" />;
};


function App() {
  const { token } = useAuth();
  return (
    <Router>
      <Navbar />
      <Routes>
        <Route path="/login" element={token ? <Navigate to="/" /> : <LoginPage />} />
        <Route path="/register" element={token ? <Navigate to="/" /> : <RegisterPage />} />
        
        <Route path="/" element={<PrivateRoute><HomePage /></PrivateRoute>} />
        {/* Placeholder pages */}
        <Route path="/recipes" element={<PrivateRoute><div>Recipes Page</div></PrivateRoute>} />
        <Route path="/friends" element={<PrivateRoute><div>Friends Page</div></PrivateRoute>} />
        <Route path="/swipe" element={<PrivateRoute><div>Swipe Page</div></PrivateRoute>} />

      </Routes>
    </Router>
  );
}

export default App;
