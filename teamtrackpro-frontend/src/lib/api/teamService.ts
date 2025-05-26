import apiClient from './client';

export interface TeamMember {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  role: string;
  projects: {
    id: string;
    name: string;
  }[];
}

export interface CreateTeamMemberRequest {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  role: string;
}

export interface UpdateTeamMemberRequest {
  firstName?: string;
  lastName?: string;
  email?: string;
  role?: string;
}

class TeamService {
  async getTeamMembers(): Promise<TeamMember[]> {
    const response = await apiClient.get<TeamMember[]>('/users');
    return response.data;
  }

  async getTeamMember(id: string): Promise<TeamMember> {
    const response = await apiClient.get<TeamMember>(`/users/${id}`);
    return response.data;
  }

  async createTeamMember(data: CreateTeamMemberRequest): Promise<TeamMember> {
    const response = await apiClient.post<TeamMember>('/users', data);
    return response.data;
  }

  async updateTeamMember(id: string, data: UpdateTeamMemberRequest): Promise<TeamMember> {
    const response = await apiClient.put<TeamMember>(`/users/${id}`, data);
    return response.data;
  }

  async deleteTeamMember(id: string): Promise<void> {
    await apiClient.delete(`/users/${id}`);
  }
}

export const teamService = new TeamService(); 