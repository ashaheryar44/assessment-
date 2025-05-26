'use client';

import { useState, useEffect } from 'react';
import { ProtectedRoute } from '@/components/auth/ProtectedRoute';
import Navigation from '@/components/layout/Navigation';
import {
  Box,
  Typography,
  Button,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  MenuItem,
  IconButton,
  Alert,
} from '@mui/material';
import { Add as AddIcon, Edit as EditIcon, Delete as DeleteIcon } from '@mui/icons-material';
import { projectService, Project, CreateProjectRequest, UpdateProjectRequest } from '@/lib/api/projectService';
import { teamService, TeamMember } from '@/lib/api/teamService';

export default function ProjectsPage() {
  const [projects, setProjects] = useState<Project[]>([]);
  const [teamMembers, setTeamMembers] = useState<TeamMember[]>([]);
  const [openDialog, setOpenDialog] = useState(false);
  const [selectedProject, setSelectedProject] = useState<Project | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [formData, setFormData] = useState<CreateProjectRequest>({
    name: '',
    description: '',
    startDate: '',
    endDate: '',
    managerId: '',
  });

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      const [projectsData, teamData] = await Promise.all([
        projectService.getProjects(),
        teamService.getTeamMembers(),
      ]);
      setProjects(projectsData);
      setTeamMembers(teamData);
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to load data');
    }
  };

  const handleOpenDialog = (project?: Project) => {
    if (project) {
      setSelectedProject(project);
      setFormData({
        name: project.name,
        description: project.description,
        startDate: project.startDate,
        endDate: project.endDate,
        managerId: project.managerId,
      });
    } else {
      setSelectedProject(null);
      setFormData({
        name: '',
        description: '',
        startDate: '',
        endDate: '',
        managerId: '',
      });
    }
    setOpenDialog(true);
  };

  const handleCloseDialog = () => {
    setOpenDialog(false);
    setSelectedProject(null);
    setFormData({
      name: '',
      description: '',
      startDate: '',
      endDate: '',
      managerId: '',
    });
  };

  const handleSubmit = async () => {
    try {
      if (selectedProject) {
        await projectService.updateProject(selectedProject.id, formData);
      } else {
        await projectService.createProject(formData);
      }
      handleCloseDialog();
      loadData();
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to save project');
    }
  };

  const handleDelete = async (id: string) => {
    if (window.confirm('Are you sure you want to delete this project?')) {
      try {
        await projectService.deleteProject(id);
        loadData();
      } catch (err: any) {
        setError(err.response?.data?.message || 'Failed to delete project');
      }
    }
  };

  return (
    <ProtectedRoute>
      <Navigation>
        <Box sx={{ flexGrow: 1 }}>
          <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
            <Typography variant="h4">Projects</Typography>
            <Button
              variant="contained"
              startIcon={<AddIcon />}
              onClick={() => handleOpenDialog()}
            >
              New Project
            </Button>
          </Box>

          {error && (
            <Alert severity="error" sx={{ mb: 2 }}>
              {error}
            </Alert>
          )}

          <TableContainer component={Paper}>
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell>Name</TableCell>
                  <TableCell>Description</TableCell>
                  <TableCell>Manager</TableCell>
                  <TableCell>Status</TableCell>
                  <TableCell>Start Date</TableCell>
                  <TableCell>End Date</TableCell>
                  <TableCell>Actions</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {projects.map((project) => (
                  <TableRow key={project.id}>
                    <TableCell>{project.name}</TableCell>
                    <TableCell>{project.description}</TableCell>
                    <TableCell>
                      {project.manager.firstName} {project.manager.lastName}
                    </TableCell>
                    <TableCell>{project.status}</TableCell>
                    <TableCell>{new Date(project.startDate).toLocaleDateString()}</TableCell>
                    <TableCell>{new Date(project.endDate).toLocaleDateString()}</TableCell>
                    <TableCell>
                      <IconButton onClick={() => handleOpenDialog(project)}>
                        <EditIcon />
                      </IconButton>
                      <IconButton onClick={() => handleDelete(project.id)}>
                        <DeleteIcon />
                      </IconButton>
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>

          <Dialog open={openDialog} onClose={handleCloseDialog} maxWidth="sm" fullWidth>
            <DialogTitle>
              {selectedProject ? 'Edit Project' : 'New Project'}
            </DialogTitle>
            <DialogContent>
              <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2, pt: 2 }}>
                <TextField
                  label="Name"
                  value={formData.name}
                  onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                  fullWidth
                />
                <TextField
                  label="Description"
                  value={formData.description}
                  onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                  multiline
                  rows={3}
                  fullWidth
                />
                <TextField
                  select
                  label="Manager"
                  value={formData.managerId}
                  onChange={(e) => setFormData({ ...formData, managerId: e.target.value })}
                  fullWidth
                >
                  {teamMembers.map((member) => (
                    <MenuItem key={member.id} value={member.id}>
                      {member.firstName} {member.lastName}
                    </MenuItem>
                  ))}
                </TextField>
                <TextField
                  label="Start Date"
                  type="date"
                  value={formData.startDate}
                  onChange={(e) => setFormData({ ...formData, startDate: e.target.value })}
                  InputLabelProps={{ shrink: true }}
                  fullWidth
                />
                <TextField
                  label="End Date"
                  type="date"
                  value={formData.endDate}
                  onChange={(e) => setFormData({ ...formData, endDate: e.target.value })}
                  InputLabelProps={{ shrink: true }}
                  fullWidth
                />
              </Box>
            </DialogContent>
            <DialogActions>
              <Button onClick={handleCloseDialog}>Cancel</Button>
              <Button onClick={handleSubmit} variant="contained">
                {selectedProject ? 'Update' : 'Create'}
              </Button>
            </DialogActions>
          </Dialog>
        </Box>
      </Navigation>
    </ProtectedRoute>
  );
} 