import React from 'react';
import ReactDOM from 'react-dom/client';
import { App as IdentityApp } from 'identity/App';

import './index.css';
import { Route, BrowserRouter as Router, Routes } from 'react-router-dom';

const App = () => (
  <Router>
    <Routes>
      <Route
        index
        element={
          <div className="container">
            <div>Name: DJPanel</div>
            <div>Framework: react</div>
            <div>Language: TypeScript</div>
            <div>CSS: Empty CSS</div>
          </div>
        }
      />
      <Route path="/Identity" element={<IdentityApp />} />
    </Routes>
  </Router>
);
const rootElement = document.getElementById('app');
if (!rootElement) throw new Error('Failed to find the root element');

const root = ReactDOM.createRoot(rootElement);

root.render(<App />);
