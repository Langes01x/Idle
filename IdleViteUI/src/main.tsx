import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import Router from './Router.tsx';
import { RouterProvider } from 'react-router';

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <RouterProvider router={Router} />
  </StrictMode>,
);