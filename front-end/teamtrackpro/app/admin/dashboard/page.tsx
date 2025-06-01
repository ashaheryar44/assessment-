"use client"

import { useState } from "react"
import { Container, Grid, Paper, Typography, Box, Card, CardContent, Fab } from "@mui/material"
import { Add, People, Assignment, Dashboard } from "@mui/icons-material"
import DashboardLayout from "@/components/layouts/dashboard-layout"
import ProjectList from "@/components/admin/project-list"
import UserList from "@/components/admin/user-list"
import CreateProjectDialog from "@/components/admin/create-project-dialog"
import CreateUserDialog from "@/components/admin/create-user-dialog"
import { useProjects } from "@/hooks/use-projects"
import { useUsers } from "@/hooks/use-users"

export default function AdminDashboard() {
  const [activeTab, setActiveTab] = useState("dashboard")
  const [createProjectOpen, setCreateProjectOpen] = useState(false)
  const [createUserOpen, setCreateUserOpen] = useState(false)
  const { projects } = useProjects()
  const { users } = useUsers()

  const stats = [
    {
      title: "Total Projects",
      value: projects.length,
      icon: <Assignment fontSize="large" />,
      color: "#1976d2",
    },
    {
      title: "Total Users",
      value: users.filter((u) => u.role !== "admin").length,
      icon: <People fontSize="large" />,
      color: "#388e3c",
    },
    {
      title: "Active Projects",
      value: projects.filter((p) => p.status === "active").length,
      icon: <Dashboard fontSize="large" />,
      color: "#f57c00",
    },
  ]

  const renderContent = () => {
    switch (activeTab) {
      case "projects":
        return <ProjectList />
      case "users":
        return <UserList />
      default:
        return (
          <Grid container spacing={3}>
            {stats.map((stat, index) => (
              <Grid item xs={12} sm={6} md={4} key={index}>
                <Card>
                  <CardContent>
                    <Box display="flex" alignItems="center" justifyContent="space-between">
                      <Box>
                        <Typography color="textSecondary" gutterBottom>
                          {stat.title}
                        </Typography>
                        <Typography variant="h4">{stat.value}</Typography>
                      </Box>
                      <Box sx={{ color: stat.color }}>{stat.icon}</Box>
                    </Box>
                  </CardContent>
                </Card>
              </Grid>
            ))}
            <Grid item xs={12}>
              <Paper sx={{ p: 3 }}>
                <Typography variant="h6" gutterBottom>
                  Recent Projects
                </Typography>
                <ProjectList limit={5} />
              </Paper>
            </Grid>
          </Grid>
        )
    }
  }

  return (
    <DashboardLayout
      title="Admin Dashboard"
      activeTab={activeTab}
      onTabChange={setActiveTab}
      tabs={[
        { id: "dashboard", label: "Dashboard", icon: <Dashboard /> },
        { id: "projects", label: "Projects", icon: <Assignment /> },
        { id: "users", label: "Users", icon: <People /> },
      ]}
    >
      <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
        {renderContent()}
      </Container>

      {activeTab === "projects" && (
        <Fab
          color="primary"
          aria-label="add project"
          sx={{ position: "fixed", bottom: 16, right: 16 }}
          onClick={() => setCreateProjectOpen(true)}
        >
          <Add />
        </Fab>
      )}

      {activeTab === "users" && (
        <Fab
          color="primary"
          aria-label="add user"
          sx={{ position: "fixed", bottom: 16, right: 16 }}
          onClick={() => setCreateUserOpen(true)}
        >
          <Add />
        </Fab>
      )}

      <CreateProjectDialog open={createProjectOpen} onClose={() => setCreateProjectOpen(false)} />

      <CreateUserDialog open={createUserOpen} onClose={() => setCreateUserOpen(false)} />
    </DashboardLayout>
  )
}
