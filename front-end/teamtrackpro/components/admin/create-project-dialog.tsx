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
import { useProjects } from "@/hooks/use-projects"
import { useUsers } from "@/hooks/use-users"

interface CreateProjectDialogProps {
  open: boolean
  onClose: () => void
}

export default function CreateProjectDialog({ open, onClose }: CreateProjectDialogProps) {
  const [formData, setFormData] = useState({
    name: "",
    description: "",
    startDate: "",
    endDate: "",
    managerId: "",
    status: "active" as const,
  })

  const { createProject } = useProjects()
  const { users } = useUsers()

  const managers = users.filter((u) => u.role === "manager")

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault()
    createProject(formData)
    setFormData({
      name: "",
      description: "",
      startDate: "",
      endDate: "",
      managerId: "",
      status: "active",
    })
    onClose()
  }

  const handleChange = (field: string, value: string) => {
    setFormData((prev) => ({ ...prev, [field]: value }))
  }

  return (
    <Dialog open={open} onClose={onClose} maxWidth="md" fullWidth>
      <form onSubmit={handleSubmit}>
        <DialogTitle>Create New Project</DialogTitle>
        <DialogContent>
          <Grid container spacing={2} sx={{ mt: 1 }}>
            <Grid item xs={12}>
              <TextField
                fullWidth
                label="Project Name"
                value={formData.name}
                onChange={(e) => handleChange("name", e.target.value)}
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
              <TextField
                fullWidth
                label="Start Date"
                type="date"
                value={formData.startDate}
                onChange={(e) => handleChange("startDate", e.target.value)}
                InputLabelProps={{ shrink: true }}
                required
              />
            </Grid>
            <Grid item xs={6}>
              <TextField
                fullWidth
                label="End Date"
                type="date"
                value={formData.endDate}
                onChange={(e) => handleChange("endDate", e.target.value)}
                InputLabelProps={{ shrink: true }}
                required
              />
            </Grid>
            <Grid item xs={6}>
              <FormControl fullWidth required>
                <InputLabel>Manager</InputLabel>
                <Select
                  value={formData.managerId}
                  onChange={(e) => handleChange("managerId", e.target.value)}
                  label="Manager"
                >
                  {managers.map((manager) => (
                    <MenuItem key={manager.id} value={manager.id}>
                      {manager.firstName} {manager.lastName}
                    </MenuItem>
                  ))}
                </Select>
              </FormControl>
            </Grid>
            <Grid item xs={6}>
              <FormControl fullWidth>
                <InputLabel>Status</InputLabel>
                <Select value={formData.status} onChange={(e) => handleChange("status", e.target.value)} label="Status">
                  <MenuItem value="active">Active</MenuItem>
                  <MenuItem value="on-hold">On Hold</MenuItem>
                  <MenuItem value="completed">Completed</MenuItem>
                </Select>
              </FormControl>
            </Grid>
          </Grid>
        </DialogContent>
        <DialogActions>
          <Button onClick={onClose}>Cancel</Button>
          <Button type="submit" variant="contained">
            Create Project
          </Button>
        </DialogActions>
      </form>
    </Dialog>
  )
}
