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
import { Visibility } from "@mui/icons-material"
import { useProjects } from "@/hooks/use-projects"
import { useAuth } from "@/hooks/use-auth"
import { format } from "date-fns"

export default function ProjectList() {
  const { projects, loading } = useProjects()
  const { user } = useAuth()

  const managerProjects = projects.filter((p) => p.managerId === user?.id)

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
              <TableCell>Start Date</TableCell>
              <TableCell>End Date</TableCell>
              <TableCell>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {managerProjects.map((project) => (
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
                <TableCell>{format(new Date(project.startDate), "MMM dd, yyyy")}</TableCell>
                <TableCell>{format(new Date(project.endDate), "MMM dd, yyyy")}</TableCell>
                <TableCell>
                  <IconButton size="small">
                    <Visibility />
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
