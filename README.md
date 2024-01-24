Desktop application for calculating carbon footprint. It features a modernistic design, visualization of calculations, an SQLite database, and the MVVM architectural pattern. Utilizing WPF, the frontend is written in XAML, while the backend is implemented in C#.
Application interface includes:
![image](https://github.com/PortfolioJankowski/CarbonFootprintDesktopWPF/assets/143873536/e5283dd8-55bf-4ca6-bb3f-cdfdc5f48922)
![image](https://github.com/PortfolioJankowski/CarbonFootprintDesktopWPF/assets/143873536/21b8a2d5-6d28-4de8-9ad7-a1b069bc31c6)
![image](https://github.com/PortfolioJankowski/CarbonFootprintDesktopWPF/assets/143873536/85acaa94-c522-4c7d-968f-1bf650767e43)
![image](https://github.com/PortfolioJankowski/CarbonFootprintDesktopWPF/assets/143873536/eb1b5975-10c3-4293-81c5-e049fa512138)
![image](https://github.com/PortfolioJankowski/CarbonFootprintDesktopWPF/assets/143873536/7f5055dd-497b-4e94-a966-e82698e3bfe1)
![image](https://github.com/PortfolioJankowski/CarbonFootprintDesktopWPF/assets/143873536/90abfcc5-1d5f-48fb-8d42-9c1756d4a8c6)
![image](https://github.com/PortfolioJankowski/CarbonFootprintDesktopWPF/assets/143873536/14fd1b75-35a6-45ea-8049-cb98a4e16b9f)

For program data, an Excel file has been created, allowing the company using the program to gather emission data and then import it into the application.
![image](https://github.com/PortfolioJankowski/CarbonFootprintDesktopWPF/assets/143873536/20fefffb-4373-4603-86ba-0e3ca16ff24a)

The program also includes the capability to export calculations to an XLSM file, which contains:
Main page with a description of the carbon footprint calculation methodology
Data sheet with a table of calculations
Report sheet, where the user can choose the year and language for generating a report in PDF format.
![image](https://github.com/PortfolioJankowski/CarbonFootprintDesktopWPF/assets/143873536/aa6ad6cf-83b2-4471-801f-6f6537e352ac)
![image](https://github.com/PortfolioJankowski/CarbonFootprintDesktopWPF/assets/143873536/576f3f28-44ed-44f4-bed2-2584102c7ab3)
![image](https://github.com/PortfolioJankowski/CarbonFootprintDesktopWPF/assets/143873536/cb06e4df-7f52-4095-9576-5d508c95162e)

Improvements to be implemented in the future:
1. Enhanced Graphs:
Implement improvements for cases when there are no calculations available. This could include displaying a message or placeholder graph to inform the user about the absence of data.
Ensure the user interface gracefully handles scenarios with no available calculations, providing a clear and user-friendly experience.
2.Refined Print Report Functionality:
Enhance the print report functionality to offer a more polished and professional output.
