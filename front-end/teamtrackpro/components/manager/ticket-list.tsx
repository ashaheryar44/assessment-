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
  IconButton,
  Typography,
  Box,
} from "@mui/material"
import { Visibility, Edit } from "@mui/icons-material"
import { useTickets } from "@/hooks/use-tickets"
import { useProjects } from "@/hooks/use-projects"
import { useUsers } from "@/hooks/use-users"
import { useAuth } from "@/hooks/use-auth"
import { format } from "date-fns"

interface TicketListProps {
  limit?: number
}

export default function TicketList({ limit }: TicketListProps) {
  const { tickets, loading } = useTickets()
  const { projects } = useProjects()
  const { users } = useUsers()
  const { user } = useAuth()

  const managerProjects = projects.filter((p) => p.managerId === user?.id)
  const managerTickets = tickets.filter((t) => managerProjects.some((p) => p.id === t.projectId))

  const getProjectName = (projectId: string) => {
    const project = projects.find((p) => p.id === projectId)
    return project?.name || "Unknown Project"
  }

  const getAssigneeName = (assignedTo: string) => {
    const assignee = users.find((u) => u.id === assignedTo)
    return assignee ? `${assignee.firstName} ${assignee.lastName}` : "Unassigned"
  }

  const getStatusColor = (status: string) => {
    switch (status) {
      case "todo":
        return "default"
      case "inprogress":
        return "warning"
      case "qa":
        return "info"
      case "done":
        return "success"
      default:
        return "default"
    }
  }

  const getPriorityColor = (priority: string) => {
    switch (priority) {
      case "low":
        return "success"
      case "medium":
        return "warning"
      case "high":
        return "error"
      case "critical":
        return "error"
      default:
        return "default"
    }
  }

  const displayTickets = limit ? managerTickets.slice(0, limit) : managerTickets

  if (loading) {
    return <Typography>Loading tickets...</Typography>
  }

  return (
    <Box>
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Title</TableCell>
              <TableCell>Project</TableCell>
              <TableCell>Assignee</TableCell>
              <TableCell>Status</TableCell>
              <TableCell>Priority</TableCell>
              <TableCell>Due Date</TableCell>
              <TableCell>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {displayTickets.map((ticket) => (
              <TableRow key={ticket.id}>
                <TableCell>
                  <Typography variant="subtitle2">{ticket.title}</Typography>
                  <Typography variant="body2" color="text.secondary">
                    {ticket.description}
                  </Typography>
                </TableCell>
                <TableCell>{getProjectName(ticket.projectId)}</TableCell>
                <TableCell>{getAssigneeName(ticket.assignedTo)}</TableCell>
                <TableCell>
                  <Chip label={ticket.status} color={getStatusColor(ticket.status) as any} size="small" />
                </TableCell>
                <TableCell>
                  <Chip label={ticket.priority} color={getPriorityColor(ticket.priority) as any} size="small" />
                </TableCell>
                <TableCell>{format(new Date(ticket.dueDate), "MMM dd, yyyy")}</TableCell>
                <TableCell>
                  <IconButton size="small">
                    <Visibility />
                  </IconButton>
                  <IconButton size="small">
                    <Edit />
                  </IconButton>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </Box>
  )
}
