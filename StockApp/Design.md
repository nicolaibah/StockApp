# Design System Specification: The High-Stakes Atelier

## 1. Overview & Creative North Star
The "Creative North Star" for this design system is **The Sovereign Ledger**.

Unlike typical financial apps that feel like cluttered spreadsheets or sterile bank portals, this system treats market data as high-end editorial content. We move beyond the "template" look by utilizing intentional asymmetry, overlapping layers, and a sophisticated depth model. The goal is to make the user feel like a curated participant in a high-stakes environment—combining the trustworthiness of a private wealth institution with the kinetic energy of a modern social platform.

We reject "standard" UI patterns in favor of breathing room, tonal shifts over borders, and a typography scale that prioritizes both narrative headlines and hyper-legible data.

---

## 2. Colors & Tonal Architecture
The palette is built on a foundation of deep, "Midnight" blues to establish institutional trust, punctuated by "Growth" greens that feel vibrant rather than neon.

### The "No-Line" Rule
**Borders are prohibited for sectioning.** To create high-end separation, designers must use background color shifts. For example, a `surface-container-low` component should sit on a `surface` background to define its bounds.

### Surface Hierarchy & Nesting
Treat the UI as a series of physical layers. We use Material Design container tokens to define "elevation" through color rather than shadows:
- **Base Layer:** `surface` (#061423)
- **Secondary Sectioning:** `surface-container-low` (#0f1c2c)
- **Primary Interaction Cards:** `surface-container` (#132030)
- **Floating/Active Elements:** `surface-container-highest` (#283646)

### The "Glass & Gradient" Rule
To inject "soul" into the data, use Glassmorphism for overlays (e.g., modals or floating nav).
- **Effect:** Apply `surface-variant` (#283646) at 60% opacity with a 20px backdrop blur.
- **Signature Gradients:** Use a subtle linear gradient (Top-Left to Bottom-Right) from `primary` (#bbc6e2) to `primary-container` (#0f1a2e) for high-impact CTAs to create a metallic, premium sheen.

---

## 3. Typography
We utilize a triad of typefaces to balance authority with technical precision.

* **Display & Headlines (Manrope):** Chosen for its modern, geometric structure. Use `display-lg` (3.5rem) for portfolio totals and `headline-md` (1.75rem) for section headers. This is our "Editorial" voice.
* **UI & Body (Inter):** The workhorse. Use `body-md` (0.875rem) for all descriptions. Inter’s tall x-height ensures readability during rapid market fluctuations.
* **Data & Labels (Work Sans):** Specifically for technical data. Use `label-md` (0.75rem) for stock tickers and percentage changes. The simplified glyphs of Work Sans ensure numbers don't "vibrate" on dark backgrounds.

---

## 4. Elevation & Depth
In this design system, depth is a tool for focus, not just decoration.

* **The Layering Principle:** Stack `surface-container-lowest` (#020f1e) cards onto `surface-container-low` (#0f1c2c) sections. This creates a "recessed" look for data tables, making them feel like they are etched into the interface.
* **Ambient Shadows:** For floating elements (like a Buy/Sell fab), use a shadow with a blur of `16` (3.5rem) and 6% opacity. The shadow color must be a tinted version of `on-surface` (#d6e4f9) to create a soft glow rather than a muddy black stain.
* **The Ghost Border Fallback:** If a boundary is strictly required for accessibility, use `outline-variant` (#44474c) at **15% opacity**. 100% opaque borders are strictly forbidden.

---

## 5. Components

### Buttons
* **Primary:** Background `secondary` (#7dffa2), Text `on-secondary` (#003918). Radius: `lg` (0.5rem).
* **Tertiary (Social):** No background. Text `primary` (#bbc6e2). On hover, use a `surface-container-high` (#1e2b3b) background shift.

### Input Fields
* **Styling:** Use `surface-container-lowest` (#020f1e) for the field background.
* **Focus State:** Instead of a thick border, use a 1px "Ghost Border" of `secondary` (#7dffa2) at 40% opacity and a subtle outer glow using `secondary_container`.

### Cards & Lists (Market Feeds)
* **The Divider Ban:** Never use lines to separate list items. Use vertical spacing (Scale `4` - 0.9rem) or alternating tonal shifts between `surface-container-low` and `surface-container`.
* **Performance Sparklines:** These should use `secondary` (#7dffa2) for growth or `error` (#ffb4ab) for loss, with a 2px stroke width and no fill.

### Additional Signature Components
* **The Social "Pit":** A floating `surface-container-highest` panel for live competition chat, utilizing glassmorphism to let the market charts peek through behind the conversation.
* **The Leaderboard Tier:** Top-ranking users are highlighted using the `primary_fixed` (#d7e2ff) background to differentiate them from the standard list.

---

## 6. Do's and Don'ts

### Do:
* **Use Asymmetry:** Place a large `display-sm` portfolio value off-center to the left, balanced by a `secondary` growth chip on the far right.
* **Prioritize Breathing Room:** When in doubt, increase spacing using the `10` (2.25rem) or `12` (2.75rem) tokens. Luxury is defined by space.
* **Color for Meaning:** Use `secondary` (#7dffa2) strictly for growth and success metrics.

### Don't:
* **Don't Use Pure Black:** Always use `surface` (#061423) to keep the "Midnight Blue" professional tone.
* **Don't Use Default Shadows:** Avoid the "dirty" look of high-opacity black shadows.
* **Don't Box Everything:** Allow charts to bleed to the edge of their containers to feel expansive and "limitless."