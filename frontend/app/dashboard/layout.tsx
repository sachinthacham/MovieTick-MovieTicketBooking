import Link from "next/link"
import { Ticket, User, Heart, Settings, LogOut } from "lucide-react"
import { Button } from "@/components/ui/Button"

export default function DashboardLayout({ children }: { children: React.ReactNode }) {
  const navItems = [
    { name: "My Tickets", href: "/dashboard/tickets", icon: Ticket },
    { name: "Profile Settings", href: "/dashboard/profile", icon: User },
    // Only mocking the routing to /profile and /tickets for the assignment
  ]

  return (
    <div className="container mx-auto px-4 py-8 md:px-8 max-w-7xl min-h-[calc(100vh-14rem)]">
      <div className="flex flex-col md:flex-row gap-8">
        
        {/* Sidebar */}
        <div className="w-full md:w-64 shrink-0 space-y-6">
          <div className="flex items-center space-x-4 p-4 rounded-2xl bg-(--muted)/50 border border-(--border)">
            <div className="h-12 w-12 rounded-full bg-(--primary)/20 text-(--primary) flex items-center justify-center font-bold text-lg">
              JS
            </div>
            <div>
              <p className="font-semibold">John Smith</p>
              <p className="text-xs text-(--muted-foreground)">john@example.com</p>
            </div>
          </div>

          <nav className="flex flex-col space-y-1">
            {navItems.map(item => (
              <Link key={item.name} href={item.href}>
                <Button variant="ghost" className="w-full justify-start text-(--foreground) hover:bg-(--muted) h-12">
                  <item.icon className="mr-3 h-5 w-5 text-(--muted-foreground)" />
                  {item.name}
                </Button>
              </Link>
            ))}
            <div className="h-px bg-(--border) my-2 w-full" />
            <Button variant="ghost" className="w-full justify-start text-red-500 hover:text-red-600 hover:bg-red-500/10 h-12">
              <LogOut className="mr-3 h-5 w-5" />
              Sign Out
            </Button>
          </nav>
        </div>

        {/* Main Content Area */}
        <div className="flex-1 space-y-6">
          {children}
        </div>
        
      </div>
    </div>
  )
}
