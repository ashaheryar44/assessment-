"use client"

import { useState } from "react"
import { Container, Grid, Paper, Typography, Card, CardContent, Box } from "@mui/material"
import { Assignment, BugReport, Dashboard, Schedule } from "@mui/icons-material"
import DashboardLayout from "@/components/layouts/dashboard-layout"
import ProjectList from "@/components/developer/project-list"
import TicketList from "@/components/developer/ticket-list"
import { useProjects } from "@/hooks/use-projects"
import { useTickets } from "@/hooks/use-tickets"
import { useAuth } from "@/hooks/use-auth"

export default function DeveloperDashboard() {
  const [activeTab, setActiveTab] = useState("dashboard")
  const { user } = useAuth()
  const { projects } = useProjects()
  const { tickets } = useTickets()

  const myTickets = tickets.filter((t) => t.assignedTo === user?.id)
  const myProjects = projects.filter((p) => myTickets.some((t) => t.projectId === p.id))

  const stats = [
    {
      title: "My Projects",
      value: myProjects.length,
      icon: <Assignment fontSize="large" />,
      color: "#1976d2",
    },
    {
      title: "My Tickets",
      value: myTickets.length,
      icon: <BugReport fontSize="large" />,
      color: "#388e3c",
    },
    {
      title: "In Progress",
      value: myTickets.filter((t) => t.status === "inprogress").length,
      icon: <Schedule fontSize="large" />,
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
                  My Recent Tickets
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
      title="Developer Dashboard"
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
    </DashboardLayout>
  )
}
