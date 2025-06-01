"use client"

import { useState } from "react"
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
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  TextField,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Grid,
} from "@mui/material"
import { Edit } from "@mui/icons-material"
import { useTickets } from "@/hooks/use-tickets"
import { useProjects } from "@/hooks/use-projects"
import { useAuth } from "@/hooks/use-auth"
import { format } from "date-fns"

interface TicketListProps {
  limit?: number
}

export default function TicketList({ limit }: TicketListProps) {
  const [editTicket, setEditTicket] = useState<any>(null)
  const [commentText, setCommentText] = useState("")
  const { tickets, loading, updateTicket, addComment } = useTickets()
  const { projects } = useProjects()
  const { user } = useAuth()

  const myTickets = tickets.filter((t) => t.assignedTo === user?.id)

  const getProjectName = (projectId: string) => {
    const project = projects.find((p) => p.id === projectId)
    return project?.name || "Unknown Project"
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

  const handleEditTicket = (ticket: any) => {
    setEditTicket({
      ...ticket,
      timeSpent: ticket.timeSpent || 0,
    })
  }

  const handleUpdateTicket = () => {
    if (editTicket) {
      updateTicket(editTicket.id, {
        status: editTicket.status,
        timeSpent: editTicket.timeSpent,
      })

      if (commentText.trim()) {
        addComment(editTicket.id, commentText, user?.id || "")
        setCommentText("")
      }

      setEditTicket(null)
    }
  }

  const displayTickets = limit ? myTickets.slice(0, limit) : myTickets

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
              <TableCell>Status</TableCell>
              <TableCell>Priority</TableCell>
              <TableCell>Due Date</TableCell>
              <TableCell>Time Spent</TableCell>
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
                <TableCell>
                  <Chip label={ticket.status} color={getStatusColor(ticket.status) as any} size="small" />
                </TableCell>
                <TableCell>
                  <Chip label={ticket.priority} color={getPriorityColor(ticket.priority) as any} size="small" />
                </TableCell>
                <TableCell>{format(new Date(ticket.dueDate), "MMM dd, yyyy")}</TableCell>
                <TableCell>{ticket.timeSpent}h</TableCell>
                <TableCell>
                  <IconButton size="small" onClick={() => handleEditTicket(ticket)}>
                    <Edit />
                  </IconButton>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>

      <Dialog open={!!editTicket} onClose={() => setEditTicket(null)} maxWidth="sm" fullWidth>
        <DialogTitle>Update Ticket</DialogTitle>
        <DialogContent>
          <Grid container spacing={2} sx={{ mt: 1 }}>
            <Grid item xs={12}>
              <Typography variant="h6">{editTicket?.title}</Typography>
              <Typography variant="body2" color="text.secondary">
                {editTicket?.description}
              </Typography>
            </Grid>
            <Grid item xs={6}>
              <FormControl fullWidth>
                <InputLabel>Status</InputLabel>
                <Select
                  value={editTicket?.status || ""}
                  onChange={(e) => setEditTicket((prev) => ({ ...prev, status: e.target.value }))}
                  label="Status"
                >
                  <MenuItem value="todo">To Do</MenuItem>
                  <MenuItem value="inprogress">In Progress</MenuItem>
                  <MenuItem value="qa">QA</MenuItem>
                  <MenuItem value="done">Done</MenuItem>
                </Select>
              </FormControl>
            </Grid>
            <Grid item xs={6}>
              <TextField
                fullWidth
                label="Time Spent (hours)"
                type="number"
                value={editTicket?.timeSpent || 0}
                onChange={(e) => setEditTicket((prev) => ({ ...prev, timeSpent: Number(e.target.value) }))}
              />
            </Grid>
            <Grid item xs={12}>
              <TextField
                fullWidth
                label="Add Comment"
                multiline
                rows={3}
                value={commentText}
                onChange={(e) => setCommentText(e.target.value)}
                placeholder="Add a comment about your progress..."
              />
            </Grid>
          </Grid>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setEditTicket(null)}>Cancel</Button>
          <Button onClick={handleUpdateTicket} variant="contained">
            Update
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  )
}
