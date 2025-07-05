import React, { useState, useCallback, useMemo } from 'react';

// Optimized ShortenUrl component using modern React patterns
const ShortenUrl = React.memo(() => {
    const [formData, setFormData] = useState({
        longUrl: '',
        shortenedUrl: '',
        isLoading: false,
        error: null
    });

    // Memoized API URL to prevent unnecessary recreations
    const apiUrl = useMemo(() => process.env.REACT_APP_API_URL, []);

    // Optimized input handler with useCallback to prevent unnecessary re-renders
    const handleInputChange = useCallback((event) => {
        const { value } = event.target;
        setFormData(prev => ({
            ...prev,
            longUrl: value,
            error: null // Clear error when user types
        }));
    }, []);

    // Optimized submit handler with proper error handling and loading states
    const handleSubmit = useCallback(async (event) => {
        event.preventDefault();
        
        if (!formData.longUrl.trim()) {
            setFormData(prev => ({ ...prev, error: 'Please enter a URL' }));
            return;
        }

        // Basic URL validation
        try {
            new URL(formData.longUrl);
        } catch {
            setFormData(prev => ({ ...prev, error: 'Please enter a valid URL' }));
            return;
        }

        setFormData(prev => ({ ...prev, isLoading: true, error: null }));

        try {
            const response = await fetch(apiUrl, {
                method: 'POST',
                headers: { 
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                },
                body: JSON.stringify({ LongURL: formData.longUrl })
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const data = await response.json();
            
            setFormData(prev => ({
                ...prev,
                shortenedUrl: data.model?.shortURL || '',
                isLoading: false,
                error: null
            }));
        } catch (error) {
            console.error('Error shortening URL:', error);
            setFormData(prev => ({
                ...prev,
                isLoading: false,
                error: 'Failed to shorten URL. Please try again.'
            }));
        }
    }, [formData.longUrl, apiUrl]);

    // Memoized shortened URL display to prevent unnecessary re-renders
    const shortenedUrlDisplay = useMemo(() => {
        if (!formData.shortenedUrl) return null;
        
        const fullUrl = `${apiUrl}/${formData.shortenedUrl}`;
        return (
            <div className="result-container" style={styles.resultContainer}>
                <label htmlFor="shortened-url">Shortened URL:</label>
                <div style={styles.urlContainer}>
                    <input
                        id="shortened-url"
                        type="text"
                        value={fullUrl}
                        readOnly
                        style={styles.readOnlyInput}
                        onClick={(e) => e.target.select()}
                    />
                    <button
                        type="button"
                        onClick={() => navigator.clipboard?.writeText(fullUrl)}
                        style={styles.copyButton}
                        title="Copy to clipboard"
                    >
                        📋
                    </button>
                </div>
                <a 
                    href={fullUrl} 
                    target="_blank" 
                    rel="noopener noreferrer"
                    style={styles.link}
                >
                    Open Link →
                </a>
            </div>
        );
    }, [formData.shortenedUrl, apiUrl]);

    return (
        <div style={styles.container}>
            <div style={styles.formContainer}>
                <h2>URL Shortener</h2>
                <form onSubmit={handleSubmit} style={styles.form}>
                    <div style={styles.inputGroup}>
                        <label htmlFor="long-url">Enter URL to shorten:</label>
                        <input
                            id="long-url"
                            type="url"
                            value={formData.longUrl}
                            onChange={handleInputChange}
                            placeholder="https://www.example.com"
                            style={styles.input}
                            disabled={formData.isLoading}
                            required
                        />
                    </div>
                    
                    <button 
                        type="submit" 
                        disabled={formData.isLoading || !formData.longUrl.trim()}
                        style={styles.submitButton}
                    >
                        {formData.isLoading ? 'Shortening...' : 'Shorten URL'}
                    </button>
                </form>

                {formData.error && (
                    <div style={styles.error} role="alert">
                        {formData.error}
                    </div>
                )}

                {shortenedUrlDisplay}
            </div>
        </div>
    );
});

// Optimized styles object (consider moving to CSS modules or styled-components for larger apps)
const styles = {
    container: {
        maxWidth: '600px',
        margin: '2rem auto',
        padding: '0 1rem'
    },
    formContainer: {
        background: '#f8f9fa',
        padding: '2rem',
        borderRadius: '8px',
        boxShadow: '0 2px 4px rgba(0,0,0,0.1)'
    },
    form: {
        display: 'flex',
        flexDirection: 'column',
        gap: '1rem'
    },
    inputGroup: {
        display: 'flex',
        flexDirection: 'column',
        gap: '0.5rem'
    },
    input: {
        padding: '0.75rem',
        border: '1px solid #ddd',
        borderRadius: '4px',
        fontSize: '1rem'
    },
    submitButton: {
        padding: '0.75rem 1.5rem',
        backgroundColor: '#007bff',
        color: 'white',
        border: 'none',
        borderRadius: '4px',
        fontSize: '1rem',
        cursor: 'pointer',
        transition: 'background-color 0.2s',
        ':hover': {
            backgroundColor: '#0056b3'
        },
        ':disabled': {
            backgroundColor: '#6c757d',
            cursor: 'not-allowed'
        }
    },
    resultContainer: {
        marginTop: '1.5rem',
        padding: '1rem',
        backgroundColor: '#d4edda',
        border: '1px solid #c3e6cb',
        borderRadius: '4px'
    },
    urlContainer: {
        display: 'flex',
        gap: '0.5rem',
        marginTop: '0.5rem'
    },
    readOnlyInput: {
        flex: 1,
        padding: '0.5rem',
        border: '1px solid #ccc',
        borderRadius: '4px',
        backgroundColor: '#fff'
    },
    copyButton: {
        padding: '0.5rem',
        border: '1px solid #ccc',
        borderRadius: '4px',
        backgroundColor: '#fff',
        cursor: 'pointer'
    },
    link: {
        display: 'inline-block',
        marginTop: '0.5rem',
        color: '#007bff',
        textDecoration: 'none'
    },
    error: {
        marginTop: '1rem',
        padding: '0.75rem',
        backgroundColor: '#f8d7da',
        color: '#721c24',
        border: '1px solid #f5c6cb',
        borderRadius: '4px'
    }
};

ShortenUrl.displayName = 'ShortenUrl';

export default ShortenUrl;