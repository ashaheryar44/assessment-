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
import { Edit, Visibility } from "@mui/icons-material"
import { useProjects } from "@/hooks/use-projects"
import { useUsers } from "@/hooks/use-users"
import { format } from "date-fns"

interface ProjectListProps {
  limit?: number
}

export default function ProjectList({ limit }: ProjectListProps) {
  const { projects, loading } = useProjects()
  const { users } = useUsers()

  const getManagerName = (managerId: string) => {
    const manager = users.find((u) => u.id === managerId)
    return manager ? `${manager.firstName} ${manager.lastName}` : "Unknown"
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

  const displayProjects = limit ? projects.slice(0, limit) : projects

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
              <TableCell>Manager</TableCell>
              <TableCell>Status</TableCell>
              <TableCell>Start Date</TableCell>
              <TableCell>End Date</TableCell>
              <TableCell>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {displayProjects.map((project) => (
              <TableRow key={project.id}>
                <TableCell>
                  <Typography variant="subtitle2">{project.name}</Typography>
                  <Typography variant="body2" color="text.secondary">
                    {project.description}
                  </Typography>
                </TableCell>
                <TableCell>{getManagerName(project.managerId)}</TableCell>
                <TableCell>
                  <Chip label={project.status} color={getStatusColor(project.status) as any} size="small" />
                </TableCell>
                <TableCell>{format(new Date(project.startDate), "MMM dd, yyyy")}</TableCell>
                <TableCell>{format(new Date(project.endDate), "MMM dd, yyyy")}</TableCell>
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
