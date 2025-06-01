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
  Avatar,
} from "@mui/material"
import { Edit, Visibility } from "@mui/icons-material"
import { useUsers } from "@/hooks/use-users"
import { format } from "date-fns"

export default function UserList() {
  const { users, loading } = useUsers()

  const getRoleColor = (role: string) => {
    switch (role) {
      case "admin":
        return "error"
      case "manager":
        return "warning"
      case "developer":
        return "info"
      default:
        return "default"
    }
  }

  const nonAdminUsers = users.filter((u) => u.role !== "admin")

  if (loading) {
    return <Typography>Loading users...</Typography>
  }

  return (
    <Box>
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>User</TableCell>
              <TableCell>Email</TableCell>
              <TableCell>Role</TableCell>
              <TableCell>Created</TableCell>
              <TableCell>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {nonAdminUsers.map((user) => (
              <TableRow key={user.id}>
                <TableCell>
                  <Box display="flex" alignItems="center" gap={2}>
                    <Avatar>
                      {user.firstName[0]}
                      {user.lastName[0]}
                    </Avatar>
                    <Box>
                      <Typography variant="subtitle2">
                        {user.firstName} {user.lastName}
                      </Typography>
                      <Typography variant="body2" color="text.secondary">
                        @{user.username}
                      </Typography>
                    </Box>
                  </Box>
                </TableCell>
                <TableCell>{user.email}</TableCell>
                <TableCell>
                  <Chip label={user.role} color={getRoleColor(user.role) as any} size="small" />
                </TableCell>
                <TableCell>{format(new Date(user.createdAt), "MMM dd, yyyy")}</TableCell>
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
