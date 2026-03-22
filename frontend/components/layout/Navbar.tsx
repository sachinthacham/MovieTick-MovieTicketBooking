import * as React from "react"
import Link from "next/link"
import { Search, MapPin, User, LogIn, Menu } from "lucide-react"

import { Button } from "@/components/ui/Button"
import { Input } from "@/components/ui/Input"
import { DropdownMenu, DropdownMenuTrigger, DropdownMenuContent, DropdownMenuItem, DropdownMenuSeparator } from "@/components/ui/Dropdown"

export function Navbar() {
  return (
    <nav className="sticky top-0 z-40 w-full border-b border-(--border) bg-(--background)/95 backdrop-blur supports-[backdrop-filter]:bg-(--background)/60">
      <div className="container mx-auto flex h-16 items-center justify-between px-4 md:px-8">
        
        {/* Brand & Desktop Links */}
        <div className="flex flex-1 items-center gap-6">
          <Link href="/" className="flex items-center space-x-2">
            <span className="text-xl font-bold tracking-tight">MovieTick</span>
          </Link>
          <div className="hidden md:flex items-center space-x-4 text-sm font-medium">
            <Link href="/" className="transition-colors hover:text-(--foreground)/80 text-(--foreground)">Movies</Link>
            <Link href="/theatres" className="transition-colors hover:text-(--foreground)/80 text-(--foreground)/60">Theatres</Link>
            <Link href="/offers" className="transition-colors hover:text-(--foreground)/80 text-(--foreground)/60">Offers</Link>
          </div>
        </div>
        
        {/* Search Bar (Desktop) */}
        <div className="hidden md:flex flex-1 items-center justify-center max-w-md px-6">
          <div className="relative w-full">
            <Search className="absolute left-2.5 top-2.5 h-4 w-4 text-(--muted-foreground)" />
            <Input 
              type="search" 
              placeholder="Search for movies, events, plays..." 
              className="w-full bg-(--muted) border-none pl-9 focus-visible:bg-(--background) focus-visible:ring-1 transition-all" 
            />
          </div>
        </div>

        {/* Right Actions */}
        <div className="flex flex-1 items-center justify-end space-x-2 md:space-x-4">
          
          {/* Location Selector */}
          <Button variant="ghost" size="sm" className="hidden sm:flex text-(--muted-foreground) px-2">
            <MapPin className="mr-2 h-4 w-4" />
            <span>Select Location</span>
          </Button>

          {/* Auth Dropdown */}
          <div className="hidden sm:block">
            <DropdownMenu>
              <DropdownMenuTrigger asChild>
                <Button variant="outline" size="sm" className="rounded-full px-3">
                  <User className="mr-2 h-4 w-4" />
                  Sign In
                </Button>
              </DropdownMenuTrigger>
              <DropdownMenuContent align="right">
                <DropdownMenuItem>
                  <Link href="/auth/login" className="flex w-full items-center">
                    <LogIn className="mr-2 h-4 w-4" />
                    Login
                  </Link>
                </DropdownMenuItem>
                <DropdownMenuItem>
                  <Link href="/auth/register" className="flex w-full items-center">
                    Register
                  </Link>
                </DropdownMenuItem>
                <DropdownMenuSeparator />
                <DropdownMenuItem>My Profile</DropdownMenuItem>
                <DropdownMenuItem>My Bookings</DropdownMenuItem>
              </DropdownMenuContent>
            </DropdownMenu>
          </div>

          {/* Mobile Menu Toggle */}
          <Button variant="ghost" size="icon" className="md:hidden">
            <Menu className="h-5 w-5" />
            <span className="sr-only">Toggle menu</span>
          </Button>
        </div>
        
      </div>
    </nav>
  )
}
