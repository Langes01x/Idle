import './App.css'
import Nav from './Nav/Nav'
import LoginPage from './Account/LoginPage';
import AccountContext, { type Account } from './Account/AccountContext';
import { useEffect, useState } from 'react';
import FetchAccount from './Account/FetchAccount';

function App() {
  const [account, setAccount] = useState<Account | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchDefaultAccount = async () => {
      try {
        setAccount(await FetchAccount());
      } catch {
        // Intentionally ignore any errors
      }
    };
    fetchDefaultAccount().then(() => setLoading(false));
  }, []);

  if (loading) {
    return (
      <div className="spinner-border" role="status">
        <span className="visually-hidden">Loading...</span>
      </div>
    );
  }

  return (
    <AccountContext value={{ account, setAccount }}>
      <header>
        <Nav />
      </header>
      <div className="container">
        <main role="main">
          <LoginPage />
        </main>
      </div>
    </AccountContext>
  );
};

export default App;