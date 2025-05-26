import { useState, useEffect } from 'react';
import { useRouter } from 'next/navigation';
import { authService, LoginRequest, SignupRequest, AuthResponse } from '../auth/authService';

export const useAuth = () => {
  const router = useRouter();
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [user, setUser] = useState<AuthResponse | null>(null);

  useEffect(() => {
    const storedUser = localStorage.getItem('user');
    if (storedUser) {
      setUser(JSON.parse(storedUser));
    }
  }, []);

  const login = async (data: LoginRequest) => {
    try {
      setIsLoading(true);
      setError(null);
      const response = await authService.login(data);
      setUser(response);
      localStorage.setItem('user', JSON.stringify(response));
      router.push('/dashboard');
    } catch (err: any) {
      setError(err.response?.data?.message || 'An error occurred during login');
    } finally {
      setIsLoading(false);
    }
  };

  const signup = async (data: SignupRequest) => {
    try {
      setIsLoading(true);
      setError(null);
      await authService.signup(data);
      router.push('/login');
    } catch (err: any) {
      setError(err.response?.data?.message || 'An error occurred during signup');
    } finally {
      setIsLoading(false);
    }
  };

  const logout = () => {
    authService.logout();
    setUser(null);
    router.push('/login');
  };

  return {
    user,
    isLoading,
    error,
    login,
    signup,
    logout,
    isAuthenticated: authService.isAuthenticated(),
  };
}; 