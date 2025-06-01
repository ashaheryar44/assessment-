"use client"

import { useState } from "react"
import { Container, Grid, Paper, Typography, Card, CardContent, Box, Fab } from "@mui/material"
import { Add, Assignment, BugReport, Dashboard } from "@mui/icons-material"
import DashboardLayout from "@/components/layouts/dashboard-layout"
import ProjectList from "@/components/manager/project-list"
import TicketList from "@/components/manager/ticket-list"
import CreateTicketDialog from "@/components/manager/create-ticket-dialog"
import { useProjects } from "@/hooks/use-projects"
import { useTickets } from "@/hooks/use-tickets"
import { useAuth } from "@/hooks/use-auth"

export default function ManagerDashboard() {
  const [activeTab, setActiveTab] = useState("dashboard")
  const [createTicketOpen, setCreateTicketOpen] = useState(false)
  const { user } = useAuth()
  const { projects } = useProjects()
  const { tickets } = useTickets()

  const managerProjects = projects.filter((p) => p.managerId === user?.id)
  const managerTickets = tickets.filter((t) => managerProjects.some((p) => p.id === t.projectId))

  const stats = [
    {
      title: "My Projects",
      value: managerProjects.length,
      icon: <Assignment fontSize="large" />,
      color: "#1976d2",
    },
    {
      title: "Total Tickets",
      value: managerTickets.length,
      icon: <BugReport fontSize="large" />,
      color: "#388e3c",
    },
    {
      title: "Open Tickets",
      value: managerTickets.filter((t) => t.status !== "done").length,
      icon: <Dashboard fontSize="large" />,
      color: "#f57c00",
    },
  ]

  const renderContent = () => {
    switch (activeTab) {
      case "projects":
        return <ProjectList />
      case "tickets":
        return <TicketList />
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
                  Recent Tickets
                </Typography>
                <TicketList limit={5} />
              </Paper>
            </Grid>
          </Grid>
        )
    }
  }

  return (
    <DashboardLayout
      title="Manager Dashboard"
      activeTab={activeTab}
      onTabChange={setActiveTab}
      tabs={[
        { id: "dashboard", label: "Dashboard", icon: <Dashboard /> },
        { id: "projects", label: "Projects", icon: <Assignment /> },
        { id: "tickets", label: "Tickets", icon: <BugReport /> },
      ]}
    >
      <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
        {renderContent()}
      </Container>

      {activeTab === "tickets" && (
        <Fab
          color="primary"
          aria-label="add ticket"
          sx={{ position: "fixed", bottom: 16, right: 16 }}
          onClick={() => setCreateTicketOpen(true)}
        >
          <Add />
        </Fab>
      )}

      <CreateTicketDialog open={createTicketOpen} onClose={() => setCreateTicketOpen(false)} />
    </DashboardLayout>
  )
}
