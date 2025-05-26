import { useEffect } from 'react';
import { useRouter } from 'next/navigation';
import { useAuth } from '@/lib/hooks/useAuth';

interface ProtectedRouteProps {
  children: React.ReactNode;
  requiredRole?: string;
}

export const ProtectedRoute: React.FC<ProtectedRouteProps> = ({ children, requiredRole }) => {
  const { isAuthenticated, user } = useAuth();
  const router = useRouter();

  useEffect(() => {
    if (!isAuthenticated) {
      router.push('/login');
    } else if (requiredRole && user?.user.role !== requiredRole) {
      router.push('/unauthorized');
    }
  }, [isAuthenticated, requiredRole, user, router]);

  if (!isAuthenticated || (requiredRole && user?.user.role !== requiredRole)) {
    return null;
  }

  return <>{children}</>;
}; 