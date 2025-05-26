import apiClient from './client';

export interface Project {
  id: string;
  name: string;
  description: string;
  startDate: string;
  endDate: string;
  status: string;
  managerId: string;
  manager: {
    id: string;
    firstName: string;
    lastName: string;
  };
}

export interface CreateProjectRequest {
  name: string;
  description: string;
  startDate: string;
  endDate: string;
  managerId: string;
}

export interface UpdateProjectRequest extends Partial<CreateProjectRequest> {
  status?: string;
}

class ProjectService {
  async getProjects(): Promise<Project[]> {
    const response = await apiClient.get<Project[]>('/projects');
    return response.data;
  }

  async getProject(id: string): Promise<Project> {
    const response = await apiClient.get<Project>(`/projects/${id}`);
    return response.data;
  }

  async createProject(data: CreateProjectRequest): Promise<Project> {
    const response = await apiClient.post<Project>('/projects', data);
    return response.data;
  }

  async updateProject(id: string, data: UpdateProjectRequest): Promise<Project> {
    const response = await apiClient.put<Project>(`/projects/${id}`, data);
    return response.data;
  }

  async deleteProject(id: string): Promise<void> {
    await apiClient.delete(`/projects/${id}`);
  }
}

export const projectService = new ProjectService(); 