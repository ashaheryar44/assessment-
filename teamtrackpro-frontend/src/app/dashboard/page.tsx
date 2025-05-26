'use client';

import { useState, useEffect } from 'react';
import { ProtectedRoute } from '@/components/auth/ProtectedRoute';
import Navigation from '@/components/layout/Navigation';
import {
  Typography,
  Grid,
  Paper,
  Box,
  List,
  ListItem,
  ListItemText,
  Divider,
  CircularProgress,
  Alert,
} from '@mui/material';
import { projectService, Project } from '@/lib/api/projectService';
import { teamService, TeamMember } from '@/lib/api/teamService';

export default function DashboardPage() {
  const [projects, setProjects] = useState<Project[]>([]);
  const [teamMembers, setTeamMembers] = useState<TeamMember[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

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
      setError(err.response?.data?.message || 'Failed to load dashboard data');
    } finally {
      setLoading(false);
    }
  };

  const getProjectStats = () => {
    const total = projects.length;
    const active = projects.filter(p => p.status === 'Active').length;
    const completed = projects.filter(p => p.status === 'Completed').length;
    const upcoming = projects.filter(p => p.status === 'Upcoming').length;

    return { total, active, completed, upcoming };
  };

  const getTeamStats = () => {
    const total = teamMembers.length;
    const managers = teamMembers.filter(m => m.role === 'Manager').length;
    const developers = teamMembers.filter(m => m.role === 'Developer').length;
    const testers = teamMembers.filter(m => m.role === 'Tester').length;

    return { total, managers, developers, testers };
  };

  if (loading) {
    return (
      <ProtectedRoute>
        <Navigation>
          <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh' }}>
            <CircularProgress />
          </Box>
        </Navigation>
      </ProtectedRoute>
    );
  }

  return (
    <ProtectedRoute>
      <Navigation>
        <Box sx={{ flexGrow: 1 }}>
          <Typography variant="h4" gutterBottom>
            Dashboard
          </Typography>

          {error && (
            <Alert severity="error" sx={{ mb: 2 }}>
              {error}
            </Alert>
          )}

          <Grid container spacing={3}>
            <Grid item xs={12} md={6} lg={4}>
              <Paper
                sx={{
                  p: 2,
                  display: 'flex',
                  flexDirection: 'column',
                  height: 240,
                }}
              >
                <Typography variant="h6" gutterBottom>
                  Project Statistics
                </Typography>
                <List>
                  <ListItem>
                    <ListItemText
                      primary="Total Projects"
                      secondary={getProjectStats().total}
                    />
                  </ListItem>
                  <Divider />
                  <ListItem>
                    <ListItemText
                      primary="Active Projects"
                      secondary={getProjectStats().active}
                    />
                  </ListItem>
                  <Divider />
                  <ListItem>
                    <ListItemText
                      primary="Completed Projects"
                      secondary={getProjectStats().completed}
                    />
                  </ListItem>
                  <Divider />
                  <ListItem>
                    <ListItemText
                      primary="Upcoming Projects"
                      secondary={getProjectStats().upcoming}
                    />
                  </ListItem>
                </List>
              </Paper>
            </Grid>

            <Grid item xs={12} md={6} lg={4}>
              <Paper
                sx={{
                  p: 2,
                  display: 'flex',
                  flexDirection: 'column',
                  height: 240,
                }}
              >
                <Typography variant="h6" gutterBottom>
                  Team Statistics
                </Typography>
                <List>
                  <ListItem>
                    <ListItemText
                      primary="Total Team Members"
                      secondary={getTeamStats().total}
                    />
                  </ListItem>
                  <Divider />
                  <ListItem>
                    <ListItemText
                      primary="Managers"
                      secondary={getTeamStats().managers}
                    />
                  </ListItem>
                  <Divider />
                  <ListItem>
                    <ListItemText
                      primary="Developers"
                      secondary={getTeamStats().developers}
                    />
                  </ListItem>
                  <Divider />
                  <ListItem>
                    <ListItemText
                      primary="Testers"
                      secondary={getTeamStats().testers}
                    />
                  </ListItem>
                </List>
              </Paper>
            </Grid>

            <Grid item xs={12} md={6} lg={4}>
              <Paper
                sx={{
                  p: 2,
                  display: 'flex',
                  flexDirection: 'column',
                  height: 240,
                }}
              >
                <Typography variant="h6" gutterBottom>
                  Recent Projects
                </Typography>
                <List>
                  {projects.slice(0, 5).map((project) => (
                    <ListItem key={project.id}>
                      <ListItemText
                        primary={project.name}
                        secondary={`${project.status} • ${new Date(project.startDate).toLocaleDateString()}`}
                      />
                    </ListItem>
                  ))}
                </List>
              </Paper>
            </Grid>
          </Grid>
        </Box>
      </Navigation>
    </ProtectedRoute>
  );
} 