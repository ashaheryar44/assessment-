"use client"

import {
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  Chip,
  Typography,
  Box,
} from "@mui/material"
import { useProjects } from "@/hooks/use-projects"
import { useTickets } from "@/hooks/use-tickets"
import { useAuth } from "@/hooks/use-auth"
import { format } from "date-fns"

export default function ProjectList() {
  const { projects, loading } = useProjects()
  const { tickets } = useTickets()
  const { user } = useAuth()

  const myTickets = tickets.filter((t) => t.assignedTo === user?.id)
  const myProjects = projects.filter((p) => myTickets.some((t) => t.projectId === p.id))

  const getTicketCount = (projectId: string) => {
    return myTickets.filter((t) => t.projectId === projectId).length
  }

  const getStatusColor = (status: string) => {
    switch (status) {
      case "active":
        return "success"
      case "completed":
        return "primary"
      case "on-hold":
        return "warning"
      default:
        return "default"
    }
  }

  if (loading) {
    return <Typography>Loading projects...</Typography>
  }

  return (
    <Box>
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Name</TableCell>
              <TableCell>Status</TableCell>
              <TableCell>My Tickets</TableCell>
              <TableCell>Start Date</TableCell>
              <TableCell>End Date</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {myProjects.map((project) => (
              <TableRow key={project.id}>
                <TableCell>
                  <Typography variant="subtitle2">{project.name}</Typography>
                  <Typography variant="body2" color="text.secondary">
                    {project.description}
                  </Typography>
                </TableCell>
                <TableCell>
                  <Chip label={project.status} color={getStatusColor(project.status) as any} size="small" />
                </TableCell>
                <TableCell>
                  <Chip label={getTicketCount(project.id)} color="primary" size="small" />
                </TableCell>
                <TableCell>{format(new Date(project.startDate), "MMM dd, yyyy")}</TableCell>
                <TableCell>{format(new Date(project.endDate), "MMM dd, yyyy")}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </Box>
  )
}
