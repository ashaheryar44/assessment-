"use client"

import type React from "react"

import { useState } from "react"
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  Button,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Grid,
} from "@mui/material"
import { useTickets } from "@/hooks/use-tickets"
import { useProjects } from "@/hooks/use-projects"
import { useUsers } from "@/hooks/use-users"
import { useAuth } from "@/hooks/use-auth"

interface CreateTicketDialogProps {
  open: boolean
  onClose: () => void
}

export default function CreateTicketDialog({ open, onClose }: CreateTicketDialogProps) {
  const [formData, setFormData] = useState({
    title: "",
    description: "",
    status: "todo" as const,
    priority: "medium" as const,
    dueDate: "",
    timeSpent: 0,
    projectId: "",
    assignedTo: "",
    type: "task" as const,
  })

  const { createTicket } = useTickets()
  const { projects } = useProjects()
  const { users } = useUsers()
  const { user } = useAuth()

  const managerProjects = projects.filter((p) => p.managerId === user?.id)
  const developers = users.filter((u) => u.role === "developer")

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault()
    createTicket(formData)
    setFormData({
      title: "",
      description: "",
      status: "todo",
      priority: "medium",
      dueDate: "",
      timeSpent: 0,
      projectId: "",
      assignedTo: "",
      type: "task",
    })
    onClose()
  }

  const handleChange = (field: string, value: string | number) => {
    setFormData((prev) => ({ ...prev, [field]: value }))
  }

  return (
    <Dialog open={open} onClose={onClose} maxWidth="md" fullWidth>
      <form onSubmit={handleSubmit}>
        <DialogTitle>Create New Ticket</DialogTitle>
        <DialogContent>
          <Grid container spacing={2} sx={{ mt: 1 }}>
            <Grid item xs={12}>
              <TextField
                fullWidth
                label="Title"
                value={formData.title}
                onChange={(e) => handleChange("title", e.target.value)}
                required
              />
            </Grid>
            <Grid item xs={12}>
              <TextField
                fullWidth
                label="Description"
                multiline
                rows={3}
                value={formData.description}
                onChange={(e) => handleChange("description", e.target.value)}
                required
              />
            </Grid>
            <Grid item xs={6}>
              <FormControl fullWidth required>
                <InputLabel>Project</InputLabel>
                <Select
                  value={formData.projectId}
                  onChange={(e) => handleChange("projectId", e.target.value)}
                  label="Project"
                >
                  {managerProjects.map((project) => (
                    <MenuItem key={project.id} value={project.id}>
                      {project.name}
                    </MenuItem>
                  ))}
                </Select>
              </FormControl>
            </Grid>
            <Grid item xs={6}>
              <FormControl fullWidth required>
                <InputLabel>Assign To</InputLabel>
                <Select
                  value={formData.assignedTo}
                  onChange={(e) => handleChange("assignedTo", e.target.value)}
                  label="Assign To"
                >
                  {developers.map((developer) => (
                    <MenuItem key={developer.id} value={developer.id}>
                      {developer.firstName} {developer.lastName}
                    </MenuItem>
                  ))}
                </Select>
              </FormControl>
            </Grid>
            <Grid item xs={4}>
              <FormControl fullWidth>
                <InputLabel>Type</InputLabel>
                <Select value={formData.type} onChange={(e) => handleChange("type", e.target.value)} label="Type">
                  <MenuItem value="bug">Bug</MenuItem>
                  <MenuItem value="feature">Feature</MenuItem>
                  <MenuItem value="task">Task</MenuItem>
                  <MenuItem value="improvement">Improvement</MenuItem>
                </Select>
              </FormControl>
            </Grid>
            <Grid item xs={4}>
              <FormControl fullWidth>
                <InputLabel>Priority</InputLabel>
                <Select
                  value={formData.priority}
                  onChange={(e) => handleChange("priority", e.target.value)}
                  label="Priority"
                >
                  <MenuItem value="low">Low</MenuItem>
                  <MenuItem value="medium">Medium</MenuItem>
                  <MenuItem value="high">High</MenuItem>
                  <MenuItem value="critical">Critical</MenuItem>
                </Select>
              </FormControl>
            </Grid>
            <Grid item xs={4}>
              <TextField
                fullWidth
                label="Due Date"
                type="date"
                value={formData.dueDate}
                onChange={(e) => handleChange("dueDate", e.target.value)}
                InputLabelProps={{ shrink: true }}
                required
              />
            </Grid>
          </Grid>
        </DialogContent>
        <DialogActions>
          <Button onClick={onClose}>Cancel</Button>
          <Button type="submit" variant="contained">
            Create Ticket
          </Button>
        </DialogActions>
      </form>
    </Dialog>
  )
}
