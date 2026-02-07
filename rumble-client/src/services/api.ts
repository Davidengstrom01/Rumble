import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5103/api',
});

// We will add an interceptor here later to attach the auth token

export default api;
