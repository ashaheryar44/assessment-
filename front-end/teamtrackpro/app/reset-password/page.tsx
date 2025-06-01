"use client"

import type React from "react"

import { useState } from "react"
import { Container, Paper, TextField, Button, Typography, Box, Alert, Link } from "@mui/material"
import { ArrowBack } from "@mui/icons-material"

export default function ResetPasswordPage() {
  const [email, setEmail] = useState("")
  const [success, setSuccess] = useState(false)
  const [loading, setLoading] = useState(false)

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setLoading(true)

    // Simulate API call
    setTimeout(() => {
      setSuccess(true)
      setLoading(false)
    }, 2000)
  }

  if (success) {
    return (
      <Container component="main" maxWidth="sm">
        <Box
          sx={{
            marginTop: 8,
            display: "flex",
            flexDirection: "column",
            alignItems: "center",
          }}
        >
          <Paper elevation={3} sx={{ padding: 4, width: "100%" }}>
            <Alert severity="success" sx={{ mb: 2 }}>
              Password reset instructions have been sent to your email address.
            </Alert>
            <Typography variant="body1" align="center" gutterBottom>
              Please check your email and follow the instructions to reset your password.
            </Typography>
            <Box textAlign="center" sx={{ mt: 3 }}>
              <Link href="/login" sx={{ display: "flex", alignItems: "center", justifyContent: "center" }}>
                <ArrowBack sx={{ mr: 1 }} />
                Back to Login
              </Link>
            </Box>
          </Paper>
        </Box>
      </Container>
    )
  }

  return (
    <Container component="main" maxWidth="sm">
      <Box
        sx={{
          marginTop: 8,
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
        }}
      >
        <Paper elevation={3} sx={{ padding: 4, width: "100%" }}>
          <Typography component="h1" variant="h4" align="center" gutterBottom>
            Reset Password
          </Typography>
          <Typography variant="body1" align="center" color="text.secondary" gutterBottom>
            Enter your email address and we'll send you instructions to reset your password.
          </Typography>

          <Box component="form" onSubmit={handleSubmit} sx={{ mt: 3 }}>
            <TextField
              margin="normal"
              required
              fullWidth
              id="email"
              label="Email Address"
              name="email"
              autoComplete="email"
              autoFocus
              value={email}
              onChange={(e) => setEmail(e.target.value)}
            />
            <Button type="submit" fullWidth variant="contained" sx={{ mt: 3, mb: 2 }} disabled={loading}>
              {loading ? "Sending..." : "Send Reset Instructions"}
            </Button>

            <Box textAlign="center">
              <Link href="/login" sx={{ display: "flex", alignItems: "center", justifyContent: "center" }}>
                <ArrowBack sx={{ mr: 1 }} />
                Back to Login
              </Link>
            </Box>
          </Box>
        </Paper>
      </Box>
    </Container>
  )
}
