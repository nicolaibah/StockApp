<!-- Design System Implementation Guide -->

# The High-Stakes Atelier: Implementation Guide

A comprehensive design system for the StockApp, inspired by "The Sovereign Ledger" philosophy.

## 📋 Table of Contents

1. [Phases Implemented](#phases-implemented)
2. [Design Token System](#design-token-system)
3. [Component Library](#component-library)
4. [Layout Patterns](#layout-patterns)
5. [Usage Examples](#usage-examples)
6. [Best Practices](#best-practices)
7. [File Structure](#file-structure)

---

## Phases Implemented

### ✅ Phase 1: Foundation & Color System
- **app.css**: Complete redesign with CSS variables, typography scale, and layer system
- **index.html**: Added font imports (Manrope, Inter, Work Sans)
- **Color tokens**: All design tokens from Design.md implemented as CSS variables
- **Typography**: Three typeface scale for different content types

### ✅ Phase 2: Component Library Refactoring
- **DesignButton.razor**: Primary, tertiary, and ghost button variants
- **DesignCard.razor**: Layered card component without borders
- **DesignDataTable.razor**: Data table with spacing instead of dividers
- **GlassmorphicPanel.razor**: Floating overlay component for modals

### ✅ Phase 3: Layout Restructuring
- **Home.razor**: Asymmetric layout with game selection
- **Game.razor**: Leaderboard, chart with time range controls, portfolio view
- **TransactionDialog.razor**: Glassmorphic modal for buy/sell transactions

### ✅ Phase 4: Visual Enhancements
- Depth & elevation system through color layering
- Glassmorphic effects for floating panels
- Gradient effects for high-impact CTAs
- Data visualization styling for charts

### ✅ Phase 5: Testing & Validation
- Build successful with no compilation errors
- All MudBlazor components replaced with custom design system components

---

## Design Token System

### Color Palette

```css
/* Surface Hierarchy - Midnight Blue Foundation */
--surface: #061423                    /* Base layer */
--surface-container-low: #0f1c2c      /* Secondary sectioning */
--surface-container: #132030          /* Primary interaction cards */
--surface-container-high: #1e2b3b     /* Elevated containers */
--surface-container-highest: #283646  /* Floating/active elements */
--surface-container-lowest: #020f1e   /* Recessed data layers */
--surface-variant: #283646            /* Glassmorphism base */

/* Semantic Colors */
--primary: #bbc6e2                    /* Primary text/accents */
--primary-container: #0f1a2e          /* Gradient destination */
--primary-fixed: #d7e2ff              /* Leaderboard highlighting */
--secondary: #7dffa2                  /* Growth/success metrics */
--error: #ffb4ab                      /* Loss/error states */
```

### Typography Scale

```css
--font-display-lg: 3.5rem             /* Portfolio totals */
--font-display-md: 2.75rem            /* Major section headers */
--font-display-sm: 2.25rem            /* Subsection headers */
--font-headline-md: 1.75rem           /* Section headers */
--font-body-md: 0.875rem              /* Body text, descriptions */
--font-label-md: 0.75rem              /* Tickers, technical data */
```

### Spacing Scale

```css
--spacing-2: 0.5rem                   /* Micro spacing */
--spacing-4: 0.9rem                   /* Small spacing */
--spacing-6: 1.5rem                   /* Standard spacing */
--spacing-8: 2rem                     /* Medium spacing */
--spacing-10: 2.25rem                 /* Large spacing (breathing) */
--spacing-12: 2.75rem                 /* Extra large spacing */
```

---

## Component Library

### Button Component (DesignButton.razor)

Three variants available:

```razor
<!-- Primary Button (Green - Secondary color) -->
<DesignButton Variant="primary" @onclick="HandleClick">
    Action
</DesignButton>

<!-- Tertiary Button (No background) -->
<DesignButton Variant="tertiary" @onclick="HandleClick">
    Secondary Action
</DesignButton>

<!-- Ghost Button (15% opacity outline) -->
<DesignButton Variant="ghost" @onclick="HandleClick">
    Minimal Action
</DesignButton>
```

**CSS Classes**: `.btn`, `.btn-primary`, `.btn-tertiary`, `.btn-ghost`

### Card Component (DesignCard.razor)

```razor
<!-- Standard Card -->
<DesignCard Layer="low" Padding="p-8">
    Content here
</DesignCard>

<!-- Elevated Card (for floating elements) -->
<DesignCard Elevated="true" Layer="highest" Padding="p-8">
    Floating content
</DesignCard>
```

**CSS Classes**: `.card`, `.card-elevated`, `.surface-container-*`

### Data Table (DesignDataTable.razor)

```razor
<DesignDataTable Title="Players Scoreboard" Items="players">
    <HeaderContent>
        <th>Rank</th>
        <th>Player</th>
        <th>Value</th>
    </HeaderContent>
    <RowTemplate Context="player">
        <td class="data-cell">@player.Rank</td>
        <td>@player.Name</td>
        <td class="data-cell text-secondary">@player.Value</td>
    </RowTemplate>
</DesignDataTable>
```

**Key Features**:
- No divider lines (uses spacing instead)
- Alternating tonal shifts on hover
- Data cells use Work Sans font
- Responsive and accessible

### Glassmorphic Panel (GlassmorphicPanel.razor)

```razor
<GlassmorphicPanel Title="Transaction Dialog">
    <p>Modal content with glassmorphism effect</p>
    
    <HeaderActions>
        <button class="btn btn-tertiary">Close</button>
    </HeaderActions>
</GlassmorphicPanel>
```

**Effect**: 60% opacity `surface-variant` + 20px backdrop blur

---

## Layout Patterns

### Asymmetric Layout (Home.razor)

```html
<div class="flex-between mb-12" style="align-items: flex-start;">
    <div>
        <h1 class="display-md">Main Title</h1>
        <p class="body-md">Subtitle</p>
    </div>
    <div class="gap-breathing flex-column" style="text-align: right;">
        <div class="display-sm text-secondary">123</div>
        <div class="label-md text-primary">Label</div>
    </div>
</div>
```

**Principles**:
- Left: Large, editorial headline (Manrope)
- Right: Numeric display with label (Work Sans + Inter)
- Breathing room creates luxury feel
- Off-center balance creates visual interest

### Layered Container Pattern (Game.razor)

```html
<div class="layer surface-container-low p-8">
    <h2 class="headline-md mb-8">Section Header</h2>
    
    <!-- Layer hierarchy -->
    <div class="surface-container-lowest">
        <!-- Recessed data -->
    </div>
</div>
```

**Layer Stack** (top to bottom):
1. `surface-container-highest` - Floating overlays
2. `surface-container-high` - Active elements
3. `surface-container` - Primary interaction
4. `surface-container-low` - Secondary sections
5. `surface-container-lowest` - Recessed data

### No-Line Divider Pattern

```html
<!-- ❌ WRONG: Using borders -->
<div style="border-top: 1px solid #ccc;">Item 1</div>

<!-- ✅ RIGHT: Using spacing + color shift -->
<div class="surface-container-low divider-space">Item 1</div>
<div class="surface-container">Item 2</div>
```

---

## Usage Examples

### Example 1: Leaderboard (Layered Design)

```razor
<div class="card">
    <h2 class="headline-md mb-8">Scoreboard</h2>
    
    @foreach (var player in players)
    {
        <div class="@(player.IsTopRanked ? "primary-fixed" : "surface-container-lowest") p-6 mb-4">
            <div class="flex-between">
                <span class="label-md">@player.Rank</span>
                <span class="body-md">@player.Name</span>
                <span class="data-cell text-secondary">@player.Value</span>
            </div>
        </div>
    }
</div>
```

### Example 2: Button State Indication

```razor
<!-- Active state: Primary button -->
<button class="@(selected == "1d" ? "btn btn-primary" : "btn btn-tertiary")">
    1 dag
</button>
```

### Example 3: Data Visualization

```razor
<!-- Sparkline colors -->
<ApexPointSeries ... Name="Growth Stocks" />
<!-- Uses: stroke = var(--secondary) #7dffa2 -->

<ApexPointSeries ... Name="Declining Stocks" />
<!-- Uses: stroke = var(--error) #ffb4ab -->
```

### Example 4: Input Focus State

```html
<!-- Focus state: Ghost border + glow -->
<input type="text" placeholder="Search..." />
<!-- CSS applies: border-color: rgba(125, 255, 162, 0.4) -->
<!-- CSS applies: box-shadow: 0 0 0 3px rgba(125, 255, 162, 0.1) -->
```

---

## Best Practices

### ✅ DO's

1. **Use Asymmetry**: Place portfolio value off-center left, metrics right
2. **Prioritize Breathing Room**: Use spacing-10 and spacing-12 for luxury feel
3. **Color for Meaning**: Green = growth (secondary), Red = loss (error)
4. **Layer Everything**: Stack surfaces from highest to lowest for depth
5. **Use Typography Hierarchy**: Manrope for headlines, Inter for body, Work Sans for data
6. **Ghost Borders**: Only when accessibility requires - 15% opacity
7. **Glassmorphism**: For floating panels with 60% opacity + 20px blur

### ❌ DON'Ts

1. **Don't Use Pure Black**: Always use `surface` (#061423) for dark base
2. **Don't Use Default Shadows**: Use ambient shadows (6% opacity, tinted)
3. **Don't Box Everything**: Allow charts to bleed to container edges
4. **Don't Use Thick Borders**: Replace with surface color shifts
5. **Don't Mix Font Families Arbitrarily**: Follow the three-font system
6. **Don't Ignore Spacing**: Luxury is defined by breathing room
7. **Don't Use 100% Opacity Outlines**: Max 40% for ghost borders

### Typography Usage

```html
<!-- Editorial Voice: Headlines -->
<h1 class="display-lg">Portfolio Value</h1>
<h2 class="headline-md">Section Title</h2>

<!-- Workhorse: Body -->
<p class="body-md">Description text</p>

<!-- Technical: Data -->
<span class="label-md data-cell">MSFT</span>
```

### Spacing Usage

```html
<!-- Micro spacing within components -->
<div class="gap-4"></div>

<!-- Standard spacing between sections -->
<div class="mb-8"></div>

<!-- Breathing room: luxury spacing -->
<div class="breathing-lg"></div>
```

---

## File Structure

```
StockApp/
├── wwwroot/
│   ├── css/
│   │   └── app.css                 # Complete design system CSS
│   ├── index.html                  # Font imports
│
├── Components/
│   ├── DesignButton.razor          # Button component (3 variants)
│   ├── DesignCard.razor            # Card component (layered)
│   ├── DesignDataTable.razor       # Table component (no dividers)
│   └── GlassmorphicPanel.razor     # Modal/overlay component
│
├── Pages/
│   ├── Home.razor                  # Game selection (asymmetric layout)
│   ├── Game.razor                  # Leaderboard + chart + portfolio
│   └── ...
│
├── Dialogs/
│   └── TransactionDialog.razor     # Buy/sell modal (glassmorphic)
│
└── Design.md                       # Design specification document
```

---

## Token Reference Quick Start

### Copy-Paste CSS Variables

```css
/* In your component or scoped CSS */
background-color: var(--surface);
color: var(--on-surface);
border-color: var(--secondary);
font-family: 'Manrope', sans-serif;
padding: var(--spacing-8);
border-radius: var(--radius-lg);
box-shadow: var(--shadow-ambient);
```

### Common Class Patterns

```html
<!-- Flex layouts -->
<div class="flex-center">Centered content</div>
<div class="flex-between">Left content | Right content</div>
<div class="flex-column gap-8">Column layout</div>

<!-- Colors -->
<span class="text-secondary">Growth indicator</span>
<span class="text-error">Loss indicator</span>

<!-- Spacing -->
<div class="mb-8 p-8 gap-breathing">Spaced content</div>

<!-- Layers -->
<div class="layer-low">Secondary section</div>
<div class="layer-high">Active element</div>
```

---

## Maintenance & Future Updates

### Adding New Colors

1. Define in `:root` CSS variables
2. Use `var(--color-name)` throughout
3. Update this documentation

### Extending Typography

1. Add new font size in `--font-*` variables
2. Create corresponding `.class-name { ... }` style
3. Document usage pattern

### Creating New Components

1. Follow component template in `Components/`
2. Use design tokens (never hardcoded colors)
3. Test for accessibility
4. Document in this guide

---

**Last Updated**: Implementation Phase 5 Complete  
**Version**: 1.0  
**Design System**: The High-Stakes Atelier
